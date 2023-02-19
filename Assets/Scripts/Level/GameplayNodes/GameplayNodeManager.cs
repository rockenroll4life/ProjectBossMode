using RockUtils.ContainerUtils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayNodeManager {
    static readonly List<GameplayNode> NO_NODES = new();

    readonly Dictionary<GameplayNode.Type, List<GameplayNode>> gameplayNodes = new();

    public void Setup() {
        Object.FindObjectsOfType<GameplayNode>()
            .ToList()
            .ForEach(node => {
                AddNode(node);
            });

        foreach(List<GameplayNode> list in gameplayNodes.Values) {
            ContainerUtils.ShuffleList(list);
        }
    }

    void AddNode(GameplayNode node) {
        if (node != null) {
            if (gameplayNodes.ContainsKey(node.type)) {
                gameplayNodes[node.type].Add(node);
            } else {
                List<GameplayNode> newList = new();
                newList.Add(node);
                gameplayNodes.Add(node.type, newList);
            }
        }
    }

    public GameplayNode GetGameplayNode(GameplayNode.Type type) {
        if (gameplayNodes.ContainsKey(type) && gameplayNodes[type].Count > 0) {
            GameplayNode node = gameplayNodes[type][0];
            gameplayNodes[type].RemoveAt(0);
            return node;
        }

        return null;
    }

    public List<GameplayNode> GetAllGameplayNodes(GameplayNode.Type type) {
        if (gameplayNodes.ContainsKey(type)) {
            List<GameplayNode> nodes = new List<GameplayNode>();
            nodes.AddRange(gameplayNodes[type]);
            gameplayNodes[type].Clear();

            return nodes;
        }

        return NO_NODES;
    }

    public void ReturnGameplayNode(GameplayNode node) {
        AddNode(node);
    }
}
