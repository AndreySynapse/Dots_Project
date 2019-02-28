using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Vector2Int _fieldSize;
    [SerializeField] private Transform _container;

    [Header("Cell configuration")]
    [SerializeField] private CellTrigger _cellTriggerPrefab;
    [SerializeField] private float _normalizedCellSize;

    private GameSession _gameSession;
    private Vector2 _cellSize;
    private Vector3 _startPosition;

    private CellTrigger[,] _field;
    
    private void Awake()
    {
        if (_gameSession == null)
            _gameSession = GameSession.Instance;

        float scaleFactor = 1f / _sprite.pixelsPerUnit;
        _cellSize = new Vector2((_sprite.rect.width / (_fieldSize.x - 1)) * scaleFactor, (_sprite.rect.height / (_fieldSize.y - 1)) * scaleFactor);

        float zPosition = _gameSession.CurrentZBuffer.Value;
        _startPosition = this.transform.position + Vector3.left * _cellSize.x * (_fieldSize.x / 2f) + Vector3.up * _cellSize.y * (_fieldSize.y / 2f) + Vector3.forward * zPosition;
        _startPosition += Vector3.right * (_cellSize.x / 2f) + Vector3.down * (_cellSize.y / 2f);
    }

    private void Start()
    {
        _field = new CellTrigger[_fieldSize.x, _fieldSize.y];

        for (int j = 0; j < _fieldSize.y; j++)
            for (int i = 0; i < _fieldSize.x; i++)
            {
                var cell = Instantiate(_cellTriggerPrefab);

                cell.transform.position = _startPosition + Vector3.right * _cellSize.x * i + Vector3.down * _cellSize.y * j;
                cell.CellCollider.size = new Vector2(_cellSize.x * _normalizedCellSize, _cellSize.y * _normalizedCellSize);
                cell.FillState = CellStates.Empty;
                cell.Index = new Vector2Int(i, j);
                cell.transform.parent = _container;

                cell.gameObject.SetActive(true);

                _field[i, j] = cell;
                cell.OnFillCell += OnFillCell;
            }
    }
    
    private void OnFillCell(CellTrigger cell)
    {
        // Граф. Внешний список - столбцы вершин. Внутренний список - в каждом столбце выдать список всех вершин. Каждая вершина - связанный список без индексов, т.е. фактически путь
        List<List<LinkedCellList>> graph = new List<List<LinkedCellList>>();

        LinkedCellList start = new LinkedCellList();
        start.Cell = cell;
        start.LastCell = null;
        FindAndSetNextPoints(start);

        List<LinkedCellList> list = new List<LinkedCellList>();
        list.Add(start);

        graph.Add(list);

        int breakCount = 100;

        List<LinkedCellList> targets = new List<LinkedCellList>();

        bool hasNext = true;
        while (hasNext)
        {
            List<LinkedCellList> nextList = new List<LinkedCellList>();

            //print("List");
            for (int i = 0; i < list.Count; i++)
            {
                LinkedCellList cellPath = list[i];
                FindAndSetNextPoints(cellPath);
                if (cellPath.NextCells.Count > 0)
                {
                    if (Contains(cellPath.NextCells, start))
                    {
                        //print("Contains");
                        for (int j = 0; j < cellPath.NextCells.Count; j++)
                        {
                            if (cellPath.NextCells[j].Cell == start.Cell)
                            {
                                targets.Add(cellPath.NextCells[j]);
                                cellPath.NextCells.RemoveAt(j);
                                j--;
                            }

                        }
                    }

                    //print("Add");
                    foreach (var item in cellPath.NextCells)
                    {
                        nextList.Add(item);
                    }
                    //nextList.Add(cellPath);
                }
            }
            
            if (nextList.Count > 0)
            {
                list = nextList;
            }
            else
            {
                hasNext = false;
            }


            breakCount--;
            if (breakCount <= 0)
            {
                print("Finish, but not good");
                break;
            }
        }

        if (targets.Count > 0)
        {
            print("Что-то найдено, перекрестимся наудачу И...");
            GameSession.Instance.CurrentSpaceRender.Draw(FindContour(targets[0], start));
        }


        //List<LinkedCellList> list = new List<LinkedCellList>();

        //LinkedCellList graph = new LinkedCellList();
        //graph.Cell = cell;
        //graph.LastCell = null;

        //list.Add(graph);

        //FindAndSetNextPoints(graph);

        //for (int i = 0; i < graph.NextCells.Count; i++)
        //{

        //}


            /*


        LinkedCellList start = new LinkedCellList();
        start.Cell = cell;
        start.LastCell = null;

        print("Start = " + start.Cell.Index);
                
        LinkedCellList list = start;
                
        FindAndSetNextPoints(list);

        print("l = " + list.Cell.Index);

        // Нужен список из LinkedCellList, т.е. фактически список путей графа

        foreach (var item in list.NextCells)
        {
            list = item;

            FindAndSetNextPoints(list);

            print(string.Format("Current = {0}, Last = {1}", list.Cell.Index, list.LastCell.Cell.Index));
            foreach (var item2 in list.NextCells)
            {
                print(item2.Cell.Index);
            }

            if (Contains(list.NextCells, start))
            {
                GameSession.Instance.CurrentSpaceRender.Draw(FindContour(list, start));
                print("Find contour");
                break;
            }
                       
            
        }
                        
            //break;

        //}
        */
    }

    private List<CellTrigger> FindContour(LinkedCellList current, LinkedCellList start)
    {
        List<CellTrigger> result = new List<CellTrigger>();

        result.Add(start.Cell);
        
        while (current != start)
        {
            result.Add(current.Cell);
            current = current.LastCell;
        }

        return result;
    }

    private bool Contains (List<LinkedCellList> list, LinkedCellList target)
    {
        foreach (var item in list)
        {
            if (item.Cell == target.Cell)
                return true;
        }

        return false;
    }

    private void FindAndSetNextPoints(LinkedCellList linkedListItem)
    {
        int minX = linkedListItem.Cell.Index.x > 0 ? linkedListItem.Cell.Index.x - 1 : 0;
        int maxX = linkedListItem.Cell.Index.x < _fieldSize.x - 1 ? linkedListItem.Cell.Index.x + 1 : _fieldSize.x - 1;
        int minY = linkedListItem.Cell.Index.y > 0 ? linkedListItem.Cell.Index.y - 1 : 0;
        int maxY = linkedListItem.Cell.Index.y < _fieldSize.y - 1 ? linkedListItem.Cell.Index.y + 1 : _fieldSize.y - 1;
                
        for (int j = minY; j <= maxY; j++)
            for (int i = minX; i <= maxX; i++)
                if (_field[i, j].FillState == linkedListItem.Cell.FillState && linkedListItem.Cell != _field[i, j])
                {
                    if (linkedListItem.LastCell == null || linkedListItem.LastCell.Cell != _field[i, j])
                    {
                        LinkedCellList item = new LinkedCellList();
                        item.Cell = _field[i, j];
                        item.LastCell = linkedListItem;

                        linkedListItem.NextCells.Add(item);
                        //print("near " + item.Cell.Index);
                    }
                }
    }
}
