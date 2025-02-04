using System;
using System.Collections.Generic;

class Program
{
    class GameState
    {
        public int Spartan117HP { get; set; } = 20;
        public int MarineHP { get; set; } = 10;
        public int Marine2HP { get; set; } = 10;
        public int EliteHP { get; set; } = 15;
        public int JackalHP { get; set; } = 8;
        public int GruntHP { get; set; } = 8;
    }

    static Stack<GameState> history = new Stack<GameState>();
    static GameState currentState = new GameState();
    static int lastAttacker = 1;

    static void SaveState()
    {
        history.Push(new GameState
        {
            Spartan117HP = currentState.Spartan117HP,
            MarineHP = currentState.MarineHP,
            Marine2HP = currentState.Marine2HP,
            EliteHP = currentState.EliteHP,
            JackalHP = currentState.JackalHP,
            GruntHP = currentState.GruntHP
        });
    }

    static void Undo()
    {
        if (history.Count > 0)
        {
            Console.WriteLine("Undo!");
            currentState = history.Pop();
        }
        else
        {
            Console.WriteLine("No more undos available!");
        }
    }

    static void PrintState()
    {
        Console.WriteLine("Player turn. (Press Ctrl+Z for Undo)");
        Console.WriteLine("Select attacker:");
        Console.WriteLine($"1. Spartan117 (HP {currentState.Spartan117HP}/20)");
        Console.WriteLine($"2. Marine (HP {currentState.MarineHP}/10)");
        Console.WriteLine($"3. Marine 2 (HP {currentState.Marine2HP}/10)");
    }

    static void Attack(int attacker, int target)
    {
        SaveState();
        int damage = (attacker == 1) ? 10 : 5;
        lastAttacker = attacker;

        string attackerName = attacker == 1 ? "Spartan117" : attacker == 2 ? "Marine" : "Marine 2";
        string targetName = target == 1 ? "Elite" : target == 2 ? "Jackal" : "Grunt";

        if (target == 1)
        {
            currentState.EliteHP -= damage;
            if (currentState.EliteHP <= 0)
            {
                Console.WriteLine("Elite has been defeated!");
            }
        }
        else if (target == 2)
        {
            currentState.JackalHP -= damage;
            if (currentState.JackalHP <= 0)
            {
                Console.WriteLine("Jackal has been defeated!");
            }
        }
        else
        {
            currentState.GruntHP -= damage;
            if (currentState.GruntHP <= 0)
            {
                Console.WriteLine("Grunt has been defeated!");
            }
        }

        Console.WriteLine($"{attackerName} attacks {targetName} for {damage} damage.");
        EnemyCounterAttack(attacker, target);
    }

    static void EnemyCounterAttack(int attacker, int target)
    {
        int enemyDamage = 5;
        string attackerName = attacker == 1 ? "Spartan117" : attacker == 2 ? "Marine" : "Marine 2";
        string targetName = target == 1 ? "Elite" : target == 2 ? "Jackal" : "Grunt";

        Console.WriteLine($"{targetName} counterattacks {attackerName} for {enemyDamage} damage.");

        if (attacker == 1)
        {
            currentState.Spartan117HP -= enemyDamage;
        }
        else if (attacker == 2)
        {
            currentState.MarineHP -= enemyDamage;
        }
        else
        {
            currentState.Marine2HP -= enemyDamage;
        }
    }

    static void Main()
    {
        while (true)
        {
            PrintState();
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Z && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                Undo();
                continue;
            }

            string input = keyInfo.KeyChar.ToString();

            if (!int.TryParse(input, out int attacker) || (attacker < 1 || attacker > 3))
                continue;

            Console.WriteLine(attacker);
            Console.WriteLine("Select target:");
            Console.WriteLine($"1. Elite (HP {currentState.EliteHP}/15)");
            Console.WriteLine($"2. Jackal (HP {currentState.JackalHP}/8)");
            Console.WriteLine($"3. Grunt (HP {currentState.GruntHP}/8)");

            keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Z && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                Undo();
                continue;
            }

            input = keyInfo.KeyChar.ToString();
            if (!int.TryParse(input, out int target) || (target < 1 || target > 3))
                continue;

            Attack(attacker, target);
        }
    }
}// peli valmis :D