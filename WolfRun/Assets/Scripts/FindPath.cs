﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapList
{
    public List<MapNode> nodes;
}

public class FindPath : MonoBehaviour
{
    [SerializeField] private List<MapList> mapLists;
    [SerializeField] private List<MapNode> path;
    [SerializeField] private MapNode start;
    [SerializeField] private MapNode finish;

    [SerializeField] private List<MapNode> openList;
    [SerializeField] private List<MapNode> closeList;
    void Start()
    {
        openList = new List<MapNode>();
        closeList = new List<MapNode>();
        initializePath(start, finish);
    }

    void Update()
    {
        
    }
    void initializePath(MapNode startNode, MapNode finishNode)
    {
        
        openList.AddRange(getNeighbor(startNode));
        closeList.Add(startNode);
        foreach (MapNode node in openList)
        {
            calculateCost(node, finishNode);
        }
        openList.Sort(delegate (MapNode A, MapNode B)
        {
            if (A.Cost > B.Cost) return 1;
            else if (A.Cost < B.Cost) return -1;
            return 0;
        });
        closeList.Add(openList[0]);
        openList.Remove(openList[0]);
        //여기까지 시작점 설정 및 주변노드 추가 작업
        //이후부터 비용 계산 및 경로 추가 작업 필요
        while(!closeList.Contains(finishNode))
        {
            openList.AddRange(getNeighbor(closeList[closeList.Count-1]));
            foreach (MapNode node in openList)
            {
                calculateCost(node, finishNode);
            }
            openList.Sort(delegate (MapNode A, MapNode B)
            {
                if (A.Cost > B.Cost) return 1;
                else if (A.Cost < B.Cost) return -1;
                return 0;
            });
            closeList.Add(openList[0]);
            openList.Remove(openList[0]);
            if(openList.Count == 0)
            {
                //길 없음
                return;
            }
        }
    }

    void calculateCost(MapNode node, MapNode finishNode)
    {
        //F(비용) = G(시작점에서 새로운 노드까지 이동 비용) + H(얻은 사각형에서 최종 목적지까지 예상 이동 비용)
        //node.Cost에 값 할당
        node.Cost = calculateMoveCost(node) + calculateHeuristic(node, finishNode);
    }
    int calculateHeuristic(MapNode node, MapNode finishNode)
    {
        //장애물을 고려하지 않고 node에서 목적지까지 가는데 드는 예상 비용(귀찮으니까 그냥 목적지 노드에서 노드까지 거리로 설정함)
        return (int)Vector3.Distance(node.gameObject.transform.position, finishNode.gameObject.transform.position);
    }
    int calculateMoveCost(MapNode node)
    {
        //부모노드에서 현재 노드로 이동하는데 드는 비용
        return node.ParentNode.MoveCost + 1 + node.Weight;
    }
    List<MapNode> getNeighbor(MapNode start)
    {
        int i = 0;
        int j = 0;

        foreach (MapList list in mapLists)
        {
            foreach (MapNode node in list.nodes)
            {
                if (start == node)
                {
                    i = mapLists.IndexOf(list);
                    j = list.nodes.IndexOf(node);
                }
            }
        }
        Debug.Log("i: " + i + " j: " + j);
        List<MapNode> _neighbor = new List<MapNode>();
        if(i > 0)
        {
            if (i < mapLists.Count-1)
            {
                if (j > 0)
                {
                    if(j<mapLists[i].nodes.Count-1)
                    {
                        if (mapLists[i - 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i - 1].nodes[j]);
                        if (mapLists[i + 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i + 1].nodes[j]);
                        if (mapLists[i].nodes[j - 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j - 1]);
                        if (mapLists[i].nodes[j + 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j + 1]);
                    }
                    else
                    {
                        if (mapLists[i - 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i - 1].nodes[j]);
                        if (mapLists[i + 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i + 1].nodes[j]);
                        if (mapLists[i].nodes[j - 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j - 1]);
                    }
                }
                else
                {
                    if (mapLists[i - 1].nodes[j].IsPath)
                        _neighbor.Add(mapLists[i - 1].nodes[j]);
                    if (mapLists[i + 1].nodes[j].IsPath)
                        _neighbor.Add(mapLists[i + 1].nodes[j]);
                    if (mapLists[i].nodes[j + 1].IsPath)
                        _neighbor.Add(mapLists[i].nodes[j + 1]);
                }
            }
            else
            {
                if (j > 0)
                {
                    if(j<mapLists[i].nodes.Count-1)
                    {
                        if (mapLists[i - 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i - 1].nodes[j]);
                        if (mapLists[i].nodes[j - 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j - 1]);
                        if (mapLists[i].nodes[j + 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j + 1]);
                    }
                    else
                    {
                        if (mapLists[i - 1].nodes[j].IsPath)
                            _neighbor.Add(mapLists[i - 1].nodes[j]);
                        if (mapLists[i].nodes[j - 1].IsPath)
                            _neighbor.Add(mapLists[i].nodes[j - 1]);
                    }
                }
                else
                {
                    if (mapLists[i - 1].nodes[j].IsPath)
                        _neighbor.Add(mapLists[i - 1].nodes[j]);
                    if (mapLists[i].nodes[j + 1].IsPath)
                        _neighbor.Add(mapLists[i].nodes[j + 1]);
                }
            }
        }
        else
        {
            if(j > 0)
            {
                if(j<mapLists[i].nodes.Count-1)
                {
                    if (mapLists[i + 1].nodes[j].IsPath)
                        _neighbor.Add(mapLists[i + 1].nodes[j]);
                    if (mapLists[i].nodes[j - 1].IsPath)
                        _neighbor.Add(mapLists[i].nodes[j - 1]);
                    if (mapLists[i].nodes[j + 1].IsPath)
                        _neighbor.Add(mapLists[i].nodes[j + 1]);
                }
                else
                {
                    if (mapLists[i + 1].nodes[j].IsPath)
                        _neighbor.Add(mapLists[i + 1].nodes[j]);
                    if (mapLists[i].nodes[j - 1].IsPath)
                        _neighbor.Add(mapLists[i].nodes[j - 1]);
                }
            }
            else
            {
                if (mapLists[i + 1].nodes[j].IsPath)
                    _neighbor.Add(mapLists[i + 1].nodes[j]);
                if (mapLists[i].nodes[j + 1].IsPath)
                    _neighbor.Add(mapLists[i].nodes[j + 1]);
            }
        }
        for(int index = 0; index < _neighbor.Count; index++)
        {
            Debug.Log(closeList.Contains(_neighbor[index]));
            if (closeList.Contains(_neighbor[index]))
            {
                Debug.Log("closeList");
                _neighbor.Remove(_neighbor[index]);
                index--;
            }
            else if (openList.Contains(_neighbor[index]))
            {
                Debug.Log("openList");
                if (calculateMoveCost(_neighbor[index]) < (mapLists[i].nodes[j].MoveCost + 1 + _neighbor[index].Weight))
                {
                    _neighbor.Remove(_neighbor[index]);
                    index--;
                }
            }
        }
        foreach(MapNode node in _neighbor)
        {
            node.ParentNode = mapLists[i].nodes[j];
        }
        return _neighbor;
    }
}
