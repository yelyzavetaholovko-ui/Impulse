using System;

namespace Impulse.Logic
{
    public class WorkoutPlanner
    {
        // 1. Метод для розрахунку спалених калорій
        public double CalculateCalories(string activityType, double weight, int durationMinutes)
        {
            if (weight <= 0) throw new ArgumentException("Вага має бути додатною");
            if (durationMinutes <= 0) throw new ArgumentException("Тривалість має бути більшою за 0");

            double met = activityType.ToLower() switch
            {
                "біг" => 8.0,
                "плавання" => 7.0,
                "силове" => 5.0,
                _ => 3.0 // легка активність за замовчуванням
            };

            // Формула: (MET * вага * 3.5) / 200 * час
            double result = (met * weight * 3.5 / 200) * durationMinutes;
            return Math.Round(result, 1);
        }

        // 2. Метод визначення рівня складності
        public string GetIntensityLevel(int avgHeartRate, int age)
        {
            if (age < 10 || age > 100) throw new ArgumentException("Некоректний вік");

            int maxHeartRate = 220 - age;
            double intensity = (double)avgHeartRate / maxHeartRate;

            if (intensity >= 0.85) return "Висока (Анаеробна)";
            if (intensity >= 0.70) return "Середня (Кардіо)";
            if (intensity >= 0.50) return "Низька (Жироспалювання)";

            return "Розминка";
        }

        // 3. Метод перевірки виконання мети
        public double GetGoalProgress(double currentKcal, double targetKcal)
        {
            if (targetKcal < 0) throw new ArgumentException("Мета не може бути від'ємною");
            if (targetKcal == 0) return 0; // Захист від ділення на нуль

            double progress = (currentKcal / targetKcal) * 100;
            return progress > 100 ? 100 : Math.Round(progress, 1);
        }
    }

    // Цей клас потрібен просто щоб Visual Studio не видавала помилку збірки (CS5001)
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}