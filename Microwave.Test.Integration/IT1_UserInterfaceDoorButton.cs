using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    /// <summary>
    /// I denne IT testes forbindelsen mellem:
    /// Userinterface og door
    /// Userinterface og button
    /// </summary>
    [TestFixture]
    public class IT1_UserInterfaceDoorButton
    {
        private UserInterface uut;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;

        private IDisplay display;
        private ILight light;

        private ICookController cooker;

        [SetUp]
        public void Setup()
        {
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            cooker = Substitute.For<ICookController>();

            uut = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker);
        }

        [Test]
        public void DoorOpen()
        {
            door.Open();
            light.Received(1).TurnOn();
        }
        [Test]
        public void DoorClosed()
        {
            door.Open();
            door.Close();
            light.Received(1).TurnOff();
        }
        [Test]
        public void ButtonClicked()
        {
            powerButton.Press();
            display.Received(1).ShowPower(50);
        }
        [Test]
        public void ExstensionOneUserPressedCancelDoingSetup()
        {
            powerButton.Press();
            startCancelButton.Press();
            display.Received(1).Clear();
        }

        [Test]
        public void ExstensionTwoUserOpensDoorDoingSetup()
        {
            powerButton.Press();
            door.Open();
            light.Received(1).TurnOn();
            display.Received(1).Clear();
        }
        [Test]
        public void ExstensionThreeUserPressedCancelDoingCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();
            cooker.Received(1).Stop();

        }
        [Test]
        public void ExstensionFourUserOpenedDoorDoingCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();
            cooker.Received(1).Stop();
        }

    }
}

