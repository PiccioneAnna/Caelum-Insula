using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameTasks
{
    public enum TaskProficiency
    {
        WontDo,
        IntentionalSabotage,
        Awful,
        Bad,
        Neutral,
        Good,
        VeryGood,
        Excellent,
        Masterwork
    }

    public enum Task
    {
        FarmWork,
        Lumbering,
        Mining,
        Crafting,
        Cooking,
        Social,
        Constructing,
        Childcare,
        Medical,
        Cleaning,
        Intellectual,
        Violent,
        Hauling
    }

    public class TaskInfo
    {
        public Task task;
        public TaskProficiency prociency;

        public string name;
        public string description;
    }

    public class TaskDictionary
    {
        public Dictionary<Task, TaskProficiency> Tasks;

        public void SetProficiency(Task task, TaskProficiency proficiency)
        {
            if(Tasks.ContainsKey(task))
            {
                Tasks[task] = proficiency;
            }
            else
            {
                Tasks.Add(task, proficiency);
            }
        }
    }
}


