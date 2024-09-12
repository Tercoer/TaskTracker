using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Task_Tracker_Net.Model {
    public class TaskTracker {

        public static bool CreateTask(ModelTask task) {
            string currentDirectory = Directory.GetCurrentDirectory();// + "\\tasks\\tasks.json";
            string jsonPath = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) + "\\tasks\\tasks.json";

            bool fileExist = File.Exists(jsonPath);
            List<ModelTask> tasks = new List<ModelTask>();

            if(fileExist) {
                string file = File.ReadAllText(jsonPath);
                tasks = JsonSerializer.Deserialize<List<ModelTask>>(file);
            }
            if(tasks.Count == 0)
                task.id = 1;
            else
                task.id = tasks.Max(t => t.id) + 1;
            tasks.Add(task);
            string serialized = JsonSerializer.Serialize(tasks);
            File.WriteAllText(jsonPath, serialized);

            return true;
        }

        public static bool UpdateTask(ModelTask task) {
            string currentDirectory = Directory.GetCurrentDirectory();// + "\\tasks\\tasks.json";
            string jsonPath = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) + "\\tasks\\tasks.json";

            if(!File.Exists(jsonPath))
                return false;

            string file = File.ReadAllText(jsonPath);

            List<ModelTask> tasks = JsonSerializer.Deserialize<List<ModelTask>>(file);

            if(tasks == null || tasks.Count == 0)
                return false;

            int index = tasks.FindIndex(x => x.id == task.id);

            if(index < 0)
                return false;

            task.createdAt = tasks[index].createdAt;
            task.taskStatus = tasks[index].taskStatus;
            tasks[index] = task;

            string serialized = JsonSerializer.Serialize(tasks);
            File.WriteAllText(jsonPath, serialized);

            return true;
        }

        public static bool UpdateStatusTask(int id, TaskStatus status) {
            string currentDirectory = Directory.GetCurrentDirectory();// + "\\tasks\\tasks.json";
            string jsonPath = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) + "\\tasks\\tasks.json";

            if(!File.Exists(jsonPath))
                return false;

            string file = File.ReadAllText(jsonPath);

            List<ModelTask> tasks = JsonSerializer.Deserialize<List<ModelTask>>(file);

            if(tasks == null || tasks.Count == 0)
                return false;

            int index = tasks.FindIndex(x => x.id == id);

            if(index < 0)
                return false;
            ModelTask task = tasks[index];
            task.taskStatus = status;
            task.updatedAt = DateTime.Now;

            tasks[index] = task;

            string serialized = JsonSerializer.Serialize(tasks);
            File.WriteAllText(jsonPath, serialized);

            return true;
        }

        public static bool RemoveTask(int id) {
            string currentDirectory = Directory.GetCurrentDirectory();// + "\\tasks\\tasks.json";
            string jsonPath = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) + "\\tasks\\tasks.json";

            if(!File.Exists(jsonPath))
                return false;

            string file = File.ReadAllText(jsonPath);

            List<ModelTask> tasks = JsonSerializer.Deserialize<List<ModelTask>>(file);

            if(tasks == null || tasks.Count == 0)
                return false;

            int index = tasks.FindIndex(x => x.id == id);

            if(index < 0)
                return false;

            tasks.RemoveAt(index);

            string serialized = JsonSerializer.Serialize(tasks);
            File.WriteAllText(jsonPath, serialized);

            return true;
        }

        public static List<ModelTask> GetTasks() {
            string currentDirectory = Directory.GetCurrentDirectory();// + "\\tasks\\tasks.json";
            string jsonPath = Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory)) + "\\tasks\\tasks.json";

            if(!File.Exists(jsonPath))
                return null;

            string file = File.ReadAllText(jsonPath);

            List<ModelTask> tasks = JsonSerializer.Deserialize<List<ModelTask>>(file);

            return tasks;
        }

    }

    public enum TaskStatus {
        TODO = 0,
        IN_PROGRESS = 1,
        DONE = 2
    }

    public class ModelTask {
        /*
            id: A unique identifier for the task
            description: A short description of the task
            status: The status of the task (todo, in-progress, done)
            createdAt: The date and time when the task was created
            updatedAt: The date and time when the task was last updated
         */
        public int id { get; set; } = 0;
        public string description { get; set; } = "";
        public TaskStatus taskStatus { get; set; } = TaskStatus.TODO;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
