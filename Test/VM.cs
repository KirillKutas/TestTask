using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Test
{
    class VM : INotifyPropertyChanged
    {
        private PersonModel selectedPerson;
        private ObservableCollection<PersonModel> personModels = new ObservableCollection<PersonModel>();
        private List<ObservableCollection<PersonModel>> allDayPerson;
        private MainWindow main;
        private DrawingGraph drawingGraph;
        private JsonSerialize jsonSerialize;
        public ObservableCollection<PersonModel> Persons { get; set; }
        public PersonModel SelectedPerson { get { return selectedPerson; } set { selectedPerson = value; OnPropertyChanged("SelectedPerson"); } }

        private MyCommand sel;
        public MyCommand Sel
        {
            get
            {
                return sel ??
                    (sel = new MyCommand(o =>
                    {
                        try
                        {
                            drawingGraph = new DrawingGraph(main);
                            drawingGraph.Drawing(SelectedPerson);
                            for (int currentRow = 0; currentRow < Persons.Count; currentRow++)
                            {
                                if ((Persons[currentRow].MinSteps < (SelectedPerson.AverageSteps * 0.8) || Persons[currentRow].MinSteps > (SelectedPerson.AverageSteps * 1.2)) && Persons[currentRow].User != selectedPerson.User || (Persons[currentRow].MaxSteps < (SelectedPerson.AverageSteps * 0.8) || Persons[currentRow].MaxSteps > (SelectedPerson.AverageSteps * 1.2)))
                                {
                                    var count = main.PersonsGrid.Items[currentRow];
                                    DataGridRow row = (DataGridRow)main.PersonsGrid.ItemContainerGenerator.ContainerFromIndex(currentRow);
                                    row.Background = new SolidColorBrush(Colors.Red);
                                }
                                else
                                {
                                    DataGridRow row = (DataGridRow)main.PersonsGrid.ItemContainerGenerator.ContainerFromIndex(currentRow);
                                    row.Background = new SolidColorBrush(Colors.White);
                                }
                            }
                            drawingGraph.DrawingMinAdnMax(selectedPerson);
                        }
                        catch(Exception ex){
                            MessageBox.Show(ex.Message);
                        }
                        
                    }));
            }
        }

        private MyCommand save;
        public MyCommand Save
        {
            get
            {
                return save ??
                    (save = new MyCommand(o =>
                    {
                        jsonSerialize = new JsonSerialize();
                        jsonSerialize.ToJson(SelectedPerson);
                    }));
            }
        }


        public VM(MainWindow m)
        {
            main = m;
            try
            {
                allDayPerson = new List<ObservableCollection<PersonModel>>();
                for(int currentDay = 1; currentDay <= 30; currentDay++)
                {
                    using (StreamReader fs = new StreamReader(@"..\\..\\TestData\\day" + currentDay + ".json"))
                    {
                        JsonTextReader jsonTextReader = new JsonTextReader(fs);
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        ObservableCollection<PersonModel> restoredPersons = jsonSerializer.Deserialize<ObservableCollection<PersonModel>>(jsonTextReader);
                        allDayPerson.Add(restoredPersons);
                    }
                }
                Persons = new ObservableCollection<PersonModel>();
                for(int currentDay = 0; currentDay < allDayPerson.Count; currentDay++)
                { 
                    for (int currentPerson = 0; currentPerson < allDayPerson[currentDay].Count; currentPerson++)
                    {
                        if(currentDay == 0)
                        {
                            Persons = allDayPerson[currentDay];
                            for (int a = 0; a < Persons.Count; a++)
                            {
                                Persons[a].MaxSteps = Persons[a].Steps;
                                Persons[a].MinSteps = Persons[a].Steps;
                                Persons[a].AllSteps.Add(Persons[a].Steps);
                            }
                            break;
                        }
                        else
                        {
                            personModels = allDayPerson[currentDay];
                            bool flag = false;
                            for(int mainPerson = 0; mainPerson < Persons.Count; mainPerson++)
                            {
                                if(Persons[mainPerson].User == personModels[currentPerson].User)
                                {
                                    if (Persons[mainPerson].MaxSteps < personModels[currentPerson].Steps)
                                    {
                                        Persons[mainPerson].MaxSteps = personModels[currentPerson].Steps;
                                    }
                                    if (Persons[mainPerson].MinSteps > personModels[currentPerson].Steps)
                                    {
                                        Persons[mainPerson].MinSteps = personModels[currentPerson].Steps;
                                    }
                                    Persons[mainPerson].Steps = Persons[mainPerson].Steps + personModels[currentPerson].Steps;
                                    Persons[mainPerson].AllSteps.Add(personModels[currentPerson].Steps);
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                Persons.Add(personModels[currentPerson]);
                                Persons[Persons.Count - 1].MaxSteps = personModels[currentPerson].Steps;
                                Persons[Persons.Count - 1].MinSteps = personModels[currentPerson].Steps;
                                Persons[Persons.Count - 1].AllSteps.Add(personModels[currentPerson].Steps);
                            }
                        }
                    }
                }
                for(int currentPerson = 0; currentPerson < Persons.Count; currentPerson++)
                {
                    Persons[currentPerson].AverageSteps = 0;
                    for(int currentStep = 0; currentStep < Persons[currentPerson].AllSteps.Count; currentStep++)
                    {
                        Persons[currentPerson].AverageSteps += Persons[currentPerson].AllSteps[currentStep];
                    }
                    Persons[currentPerson].AverageSteps = Persons[currentPerson].AverageSteps / Persons[currentPerson].AllSteps.Count;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public VM()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
