﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Daily3Goals
{
    public partial class GoalsPage : ContentPage
    {
        // Our 3 goals
        List<Goal> goals;
        // Current date
        DateTime date;

        public GoalsPage()
        {
            goals = new List<Goal>();

            // All goals start as this
            Goal goalTemplate = new Goal {
                Date = DateTime.Now,
                Name = ""
            };

            goals.Add(goalTemplate);
            goals.Add(goalTemplate);
            goals.Add(goalTemplate);

            date = DateTime.Now;

            DoneCommand = new Command<int>(index => {
                // Toggle the done property when the done button is pressed
                Goal goal = goals[index];
                goal.Done = !goal.Done;
                goals[index] = goal;
                OnPropertyChanged("Goal" + (index + 1) + "Done");
            });

            // Load goals from file if any
            Load();

            // Save goal when app goes to background
            MessagingCenter.Subscribe<App>(this, "OnSleep", sender => {
                Save();
            });

            InitializeComponent();
        }

        public string Date => date.ToString("dddd, MMMM dd, yyyy");

        public string Goal1 {
            get => goals[0].Name;
            set {
                if (goals[0].Name != value) {
                    Goal newGoal = goals[0];
                    newGoal.Name = value;
                    goals[0] = newGoal;
                    OnPropertyChanged("Goal1");
                }
            }
        }

        public string Goal2 {
            get => goals[1].Name;
            set {
                if (goals[1].Name != value) {
                    Goal newGoal = goals[1];
                    newGoal.Name = value;
                    goals[1] = newGoal;
                    OnPropertyChanged("Goal2");
                }
            }
        }

        public string Goal3 {
            get => goals[2].Name;
            set {
                if (goals != null & goals[2].Name != value) {
                    Goal newGoal = goals[2];
                    newGoal.Name = value;
                    goals[2] = newGoal;
                    OnPropertyChanged("Goal3");
                }
            }
        }

        public bool Goal1Done => goals[0].Done;

        public bool Goal2Done => goals[1].Done;

        public bool Goal3Done => goals[2].Done;

        public ICommand DoneCommand { get; private set; }

        void Load()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "goals.json");
            if (File.Exists(fileName)) {
                List<Goal> savedGoals = JsonConvert.DeserializeObject<List<Goal>>(File.ReadAllText(fileName));
                // Only populate goal if it's the same date as today
                if (savedGoals[0].Date.Year == DateTime.Now.Year
                    && savedGoals[0].Date.Month == DateTime.Now.Month
                    && savedGoals[0].Date.Day == DateTime.Now.Day) {
                    goals = savedGoals;
                }
            }
        }

        void Save()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "goals.json");
            string json = JsonConvert.SerializeObject(goals);
            File.WriteAllText(fileName, json);
        }
    }
}