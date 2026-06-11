using System;

namespace Impulse.Metrics
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double CurrentWeight { get; set; } // Поточна вага
        public double TargetWeight { get; set; }  // Цільова вага
        public double Height { get; set; }
    }

    public class ProgressTracker
    {
        // Метод 1: Розрахунок відсотка втраченої ваги (Умови + Винятки)
        public double CalculateWeightLossPercentage(double startWeight, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Користувач не може бути null.");
            if (startWeight <= 0 || user.CurrentWeight <= 0)
                throw new ArgumentException("Вага повинна бути більшою за 0.");
            if (user.CurrentWeight > startWeight)
                throw new ArgumentException("Поточна вага не може бути більшою за стартову при схудненні.");

            double totalLost = startWeight - user.CurrentWeight;
            double percentage = (totalLost / startWeight) * 100;
            return Math.Round(percentage, 2);
        }

        // Метод 2: Розрахунок днів до мети (Розгалуження)
        public int CalculateDaysToGoal(User user, double weeklyDeficit)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Користувач не може бути null.");
            if (user.CurrentWeight <= user.TargetWeight)
                return 0; // Мета вже досягнута
            if (weeklyDeficit <= 0)
                throw new ArgumentException("Дефіцит калорій повинен бути додатнім для спалення жиру.");

            // 1 кг жиру ~ 7700 ккал
            double weightToLose = user.CurrentWeight - user.TargetWeight;
            double totalCaloriesNeeded = weightToLose * 7700;
            double dailyDeficit = weeklyDeficit / 7.0;

            double days = totalCaloriesNeeded / dailyDeficit;
            return (int)Math.Ceiling(days);
        }

        // Метод 3: Оновлення статусу мети (Умови + Адаптивний цикл)
        public string UpdateGoalStatus(User user, int weeksPassed, double actualLoss)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Користувач не може бути null.");
            if (weeksPassed <= 0)
                throw new ArgumentException("Кількість пройдених тижнів має бути більшою за 0.");

            double expectedWeeklyLoss = 0.5; // Безпечна норма (0.5 кг на тиждень)
            double currentLossRate = actualLoss / weeksPassed;
            double weightLeft = user.CurrentWeight - user.TargetWeight;

            // Нетривіальна логіка: якщо прогрес замалий, симулюємо адаптивний перерахунок 
            // прогнозованого залишку тижнів з динамічним коригуванням плану
            int adjustedWeeks = 0;
            while (weightLeft > 0)
            {
                if (currentLossRate < expectedWeeklyLoss)
                {
                    // Якщо прогрес повільний, система ШІ адаптивно "подовжує" термін,
                    // але закладає мікро-прискорення за рахунок майбутньої корекції програми
                    weightLeft -= (currentLossRate * 1.1);
                }
                else
                {
                    weightLeft -= currentLossRate;
                }
                adjustedWeeks++;

                // Захист від нескінченного циклу (якщо вага взагалі стоїть на місці)
                if (adjustedWeeks > 52)
                {
                    return "Критично повільний прогрес. Потрібно змінити план тренувань!";
                }
            }

            if (currentLossRate >= expectedWeeklyLoss)
            {
                return $"Чудовий прогрес! Орієнтовно залишилось тижнів: {adjustedWeeks}";
            }
            return $"Прогрес повільний, терміни зміщено. Орієнтовно залишилось тижнів: {adjustedWeeks}";
        }
    }
}
