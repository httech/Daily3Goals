﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Daily3Goals
{
    public class GoalsViewModel : INotifyPropertyChanged
    {
        const int MaxGoals = 3;

        Goal[] goals;
        DateTime date;

        public GoalsViewModel(Goal[] newGoals, DateTime date)
        {
            goals = new Goal[3];
            this.date = date;

            if (newGoals == null) {
                newGoals = goals;
            }

            // Keep limit at 3 and always have 3
            for (int i = 0; i < MaxGoals; i++) {
                if (i < goals.Length) {
                    // If newGoals have >= 3 goals
                    goals[i] = newGoals[i];
                } else {
                    // Create new goal with passed in date if newGoals have < 3 goals
                    Goal newGoal = new Goal {
                        Date = date
                    };
                    goals[i] = newGoal;
                }
            }

            DoneCommand = new Command<int>(index => {
                // Toggle the done property when the done button is pressed
                Goal goal = goals[index];
                goal.Done = !goal.Done;
                goals[index] = goal;

                // Not always a good idea but this works fine for this
                OnPropertyChanged("Goal" + (index + 1) + "DoneButtonColor");

                GoalsChanged?.Invoke(this, goals);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Let Page know when a goal has changed. Passes all goals back for now.
        public event EventHandler<Goal[]> GoalsChanged;

        public string Date => date.ToString("dddd, MMMM dd, yyyy");

        public string Goal1EntryText {
            get => goals[0].Name;
            set {
                if (goals[0].Name != value) {
                    Goal newGoal = goals[0];
                    newGoal.Name = value;
                    goals[0] = newGoal;

                    OnPropertyChanged();

                    GoalsChanged?.Invoke(this, goals);
                }
            }
        }

        public string Goal2EntryText {
            get => goals[1].Name;
            set {
                if (goals[1].Name != value) {
                    Goal newGoal = goals[1];
                    newGoal.Name = value;
                    goals[1] = newGoal;

                    OnPropertyChanged();

                    GoalsChanged?.Invoke(this, goals);
                }
            }
        }

        public string Goal3EntryText {
            get => goals[2].Name;
            set {
                if (goals[2].Name != value) {
                    Goal newGoal = goals[2];
                    newGoal.Name = value;
                    goals[2] = newGoal;

                    OnPropertyChanged();

                    GoalsChanged?.Invoke(this, goals);
                }
            }
        }

        public Color Goal1DoneButtonColor => GetDoneColor(goals[0].Done);

        public Color Goal2DoneButtonColor => GetDoneColor(goals[1].Done);

        public Color Goal3DoneButtonColor => GetDoneColor(goals[2].Done);

        public ICommand DoneCommand { get; private set; }

        // No need for DoneToColorConverter anymore
        Color GetDoneColor(bool done)
        {
            return done ? Color.Green : Color.LightGray;
        }

        void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
