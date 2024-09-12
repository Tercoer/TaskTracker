using System;
using System.Collections.Generic;
using System.Linq;
using Task_Tracker_Net.Model;

namespace Task_Tracker_Net {
    internal class Program {

        public enum Commands {
            LIST = 0,
            ADD = 1, 
            UPDATE = 2,
            DELETE = 3,
            MARK_DONE = 4,
            MARK_IN_PROGRESS = 5,
            MARK_TODO = 6,
            CLS = 7
        };
        
        static void Main(string[] args) {
            loadMenu();
        }

        private static void loadMenu() {
            //ModelTask t = new ModelTask();
            //t.createdAt = DateTime.Now;
            //t.taskStatus = TaskStatus.DONE;
            //t.description = "changed by index";
            ////TaskTracker.RemoveTask(t);
            //TaskTracker.CreateTask(t);
            waitForKeyboard();
        }

        private static void waitForKeyboard() {

            List<Commands> listCommands = Enum.GetValues(typeof(Commands)).Cast<Commands>().ToList();
            bool isRunning = true;

            //string vname = Enum.GetName(typeof(commands), values[0]);

            while(isRunning) {

                Console.Write("task-cli > ");
                string input = Console.ReadLine();

                if(input == "")
                    continue;

                input = input.ToUpper().Trim();
                string[] positionalArguments = input.Split(' ');
                if(positionalArguments == null || positionalArguments.Length == 0) {
                    Console.WriteLine("Wrong input");
                    continue;
                } 
                string command = positionalArguments[0].Replace('-', '_');
                int indexCommand = listCommands.FindIndex(c => c.ToString() == command);
                if(indexCommand < 0) {
                    Console.WriteLine("Command not found");
                    continue;
                } 
                
                Commands cmd = listCommands[indexCommand];

                switch(cmd) {
                    case Commands.LIST:
                        List<ModelTask> tasks = TaskTracker.GetTasks();
                        if(tasks == null || tasks.Count == 0) {
                            Console.WriteLine("Tasks not found");
                            continue;
                        }
                        if(positionalArguments.Length == 1) {
                            //MOSTRAR LISTADO ORIGINAL
                            printList(tasks);
                            break;
                        }

                        if(positionalArguments.Length != 2) {
                            Console.WriteLine("Wrong command argument");
                            break;
                        }
                        List<TaskStatus> taskStatus = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();
                        string status = positionalArguments[1];

                        int indexStatus = taskStatus.FindIndex(s => s.ToString() == status);
                        if(indexStatus < 0)
                            break;
                        TaskStatus statusValue = taskStatus[indexStatus];
                        List<ModelTask> filteredTasks = tasks.Where(t => t.taskStatus == statusValue).ToList();
                        //MOSTRAR EL LISTADO FILTRADO
                        printList(filteredTasks);
                        break;
                    case Commands.ADD:
                        if(positionalArguments.Length < 2)
                            break;

                        ModelTask model = new ModelTask();
                        model.taskStatus = TaskStatus.TODO;
                        model.description = "\""+input.Remove(0, 4).Trim()+"\"";
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        TaskTracker.CreateTask(model);
                        break;
                    case Commands.UPDATE:
                        if(positionalArguments.Length < 3)
                            break;
                        bool isNumberUPD = int.TryParse(positionalArguments[1], out int idUPD);
                        if(!isNumberUPD)
                            break;             //command | id
                        string updateCommand = positionalArguments[0] + " " + positionalArguments[1];
                        ModelTask modelUPD = new ModelTask();
                        modelUPD.id = idUPD;
                        modelUPD.description = "\""+input.Remove(0, updateCommand.Length).Trim()+"\"";
                        modelUPD.updatedAt = DateTime.Now;
                        TaskTracker.UpdateTask(modelUPD);
                        break;
                    case Commands.DELETE:
                        if(positionalArguments.Length != 2)
                            break;
                        bool isNumberDEL = int.TryParse(positionalArguments[1], out int idDEL);
                        if(!isNumberDEL)
                            break;

                        TaskTracker.RemoveTask(idDEL);
                        break;
                    case Commands.MARK_DONE:
                        if(positionalArguments.Length != 2)
                            break;
                        bool isNumberDone = int.TryParse(positionalArguments[1], out int idDone);
                        if(!isNumberDone)
                            break;
                        TaskTracker.UpdateStatusTask(idDone, TaskStatus.DONE);

                        break;
                    case Commands.MARK_IN_PROGRESS:
                        if(positionalArguments.Length != 2)
                            break;
                        bool isNumberProgress = int.TryParse(positionalArguments[1], out int idProgress);
                        if(!isNumberProgress)
                            break;
                        TaskTracker.UpdateStatusTask(idProgress, TaskStatus.IN_PROGRESS);

                        break;
                    case Commands.MARK_TODO:
                        if(positionalArguments.Length != 2)
                            break;
                        bool isNumberToDo = int.TryParse(positionalArguments[1], out int idToDo);
                        if(!isNumberToDo)
                            break;

                        TaskTracker.UpdateStatusTask(idToDo, TaskStatus.TODO);
                        break;
                    case Commands.CLS:
                        Console.Clear();
                        break;
                }

            }

        }

        private static void printList(List<ModelTask> tasks) {

            foreach (ModelTask item in tasks) {
                Console.WriteLine($"ID:{item.id} " +
                                  $"Description: {item.description} " +
                                  $"Status {Enum.GetName(typeof(TaskStatus), item.taskStatus)} " +
                                  $"CreatedAt: {item.createdAt} " +
                                  $"UpdatedAt: {item.updatedAt} ");
            }

        }

    }
}
