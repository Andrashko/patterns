using System;
namespace Structural.Game
{
    /*
    Приклад використання шаблону Декоратор 
    для реалізації системи пошкоджень в грі.
    Клас персонаж має такі характеристики:
    1. життя
    2. сила удару
    3. ім'я
    Необхідно реалізувати методи
    1. прийняти шкоду (змншити життя)
    2. вдарити іншого персонажа
    3. померти
    */

    interface IDamageActor
    {
        void TakeDamage(int Damage);
        void Hit(IDamageActor other);

        bool IsDead();
    }

    class Character : IDamageActor
    {
        protected int HealthPoints;
        protected int AttackDamage;
        protected string Name;
        public Character(string Name, int HealthPoints, int AttackDamage)
        {
            this.Name = Name;
            this.HealthPoints = HealthPoints;
            this.AttackDamage = AttackDamage;
        }

        public void TakeDamage(int Damage)
        {
            this.HealthPoints -= Damage;
            Console.WriteLine($"{this.Name} take a hit {Damage}. {this.HealthPoints} healthpoits left");
            if (this.IsDead())
                this.Die();
        }

        public bool IsDead()
        {
            return this.HealthPoints <= 0;
        }
        protected void Die()
        {
            Console.WriteLine($"{this.Name} is dead!");
        }

        public void Hit(IDamageActor other)
        {
            other.TakeDamage(this.AttackDamage);
        }
    }

    /*
    декоратор бафу використовується для 
     зменшення отримання шкодина х%
    */
    class CharacterBuff : IDamageActor
    {
        protected IDamageActor DamageActor;
        public CharacterBuff(IDamageActor DamageActor)
        {
            this.DamageActor = DamageActor;
        }

        public virtual void Hit(IDamageActor other)
        {
            this.DamageActor.Hit(other);
        }
        public virtual void TakeDamage(int Damage)
        {
            this.DamageActor.TakeDamage(Damage);
        }

        public bool IsDead()
        {
            return this.DamageActor.IsDead();
        }

        public IDamageActor Undecorate()
        {
            return this.DamageActor;
        }
    }

    class DefenceBuff : CharacterBuff
    {
        protected double ReduceDamageCoeffitient;
        public DefenceBuff(IDamageActor DamageActor, int DefencePercet) : base(DamageActor)
        {
            this.ReduceDamageCoeffitient = 1 - DefencePercet / 100.0;
        }

        public override void TakeDamage(int Damage)
        {
            base.TakeDamage((int) Math.Floor(Damage * this.ReduceDamageCoeffitient));
        }
    }
    /* 
    необхідно реалізувати баф для шбільшення нанесення шкоди
    */
}