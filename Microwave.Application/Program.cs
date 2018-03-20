using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Application
{
    class Program
    {
        private CookController cooker;

        private IUserInterface ui;
        private ITimer timer;
        private IDisplay display;
        private IPowerTube powerTube;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;
        private ILight light;

        private IOutput output;

        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("RUNNING EXTENSION ONE");
            p.Extension1();
            Console.WriteLine("RUNNING EXTENSION TWO");
            p.Extension2();
            Console.WriteLine("RUNNING EXTENSION THREE");
            p.Extension3();
            Console.WriteLine("RUNNING EXTENSION FOUR");
            p.Extension4();
            Console.WriteLine("RUNNING FULL APPLICATION");
            p.Run();
            Console.ReadKey();
        }

        public Program()
        {
            output = new Output();

            //Cooker
            timer = new Timer();
            display = new Display(output);
            powerTube = new PowerTube(output);

            cooker = new CookController(timer, display, powerTube, null);


            //Userinterface
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            light = new Light(output);
            ui = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker);

            cooker.UI = ui;
        }

        public void Run()
        {
            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
        }
        public void Extension1()
        {
            door.Open();
            door.Close();
            powerButton.Press();
            startCancelButton.Press();
        }
        public void Extension2()
        {
            door.Open();
            door.Close();
            powerButton.Press();
            door.Open();
        }
        public void Extension3()
        {
            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Pressing cancel!");
            startCancelButton.Press();
        }
        public void Extension4()
        {
            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Opening door!");
            door.Open();
        }
    }
}
