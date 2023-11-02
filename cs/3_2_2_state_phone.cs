using System;

namespace Behavioral.State
{
    // Дії які повинен виконувати телефон при натисканні кнопки і спробі зателефонувати: 

    interface IPhoneState
    {
        void PressButton(Phone phone);
        void DialNumber(Phone phone, string number);
    }

    class LockedPhoneState : IPhoneState
    {
        public void PressButton(Phone phone)
        {
            phone.State = new UnlockedPhoneState();
            Console.WriteLine("Phone is unlocked");
        }

        public void DialNumber(Phone phone, string number)
        {
            phone.ShowMessage("Cannot to Dial. Phone is Locked");
        }
    }

    class UnlockedPhoneState : IPhoneState
    {
        public void PressButton(Phone phone)
        {
            phone.State = new LockedPhoneState();
            Console.WriteLine("Phone is locked");
        }

        public void DialNumber(Phone phone, string number)
        {
            phone.ShowMessage($"Dialing {number}");
        }
    }


    class Phone
    {

        // стан конкретного телефону 
        public IPhoneState State = new LockedPhoneState();


        public void ShowMessage(string message)
        {
            Console.WriteLine($"Message: {message}");
        }
        //описуємо методи, які доступні користувачу
        public void PressButton()
        {
            State.PressButton(this);
        }

        public void DialNumber(string number)
        {
            State.DialNumber(this, number);
        }
    }
}
