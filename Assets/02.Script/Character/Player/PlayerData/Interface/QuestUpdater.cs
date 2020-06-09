using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface QuestUpdater
{
    void AddObserver(QuestObserver observer);
    void DeleteObserver(QuestObserver observer);
}
