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
            phone.SetState(new UnlockedPhoneState());
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
            phone.SetState(new LockedPhoneState());
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
        private IPhoneState _state = new LockedPhoneState();

        public void SetState(IPhoneState phoneState)
        {
            _state = phoneState;
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine($"Message: {message}");
        }
        //описуємо методи, які доступні користувачу
        public void PressButton()
        {
            _state.PressButton(this);
        }

        public void DialNumber(string number)
        {
            _state.DialNumber(this, number);
        }
    }
}
