using System;
using System.Collections.Generic;
using Task_Tracker_Net.Model;

namespace Task_Tracker_Net {
    internal class Program {

        public enum commands {
            LIST = 0,
            ADD = 1, 
            UPDATE = 2,
            DELETE = 3,

        };
        static void Main(string[] args) {
            loadMenu();
        }

        private static void loadMenu() {
            ModelTask t = new ModelTask();
            t.createdAt = DateTime.Now;
            t.taskStatus = TaskStatus.DONE;
            t.description = "changed by index";
            //TaskTracker.RemoveTask(t);
            TaskTracker.CreateTask(t);
        }

        private static void waitForKeyboard() {
            Console.WriteLine("Welcome");
            bool isRunning = true;
            while(isRunning) {

                switch() {

                }


            }

        }

    }
}
