using System;

namespace ImpulseApp
{
    public class BiometricsCalculator
    {
        // Метод 1: Розрахунок Індексу Маси Тіла (ІМТ)
        public double CalculateBMI(double weightKg, double heightCm)
        {
            if (weightKg <= 0 || heightCm <= 0)
                throw new ArgumentException("Вага та зріст повинні бути більшими за нуль.");

            double heightM = heightCm / 100.0;
            double bmi = weightKg / (heightM * heightM);
            return Math.Round(bmi, 2);
        }

        // Метод 2: Розрахунок добової норми калорій (Формула Харріса-Бенедикта)
        public double CalculateDailyCalories(double weightKg, double heightCm, int age, string gender)
        {
            if (weightKg <= 0 || heightCm <= 0 || age <= 0)
                throw new ArgumentException("Показники повинні бути додатними числами.");

            double bmr;
            if (gender.ToLower() == "male" || gender.ToLower() == "чоловік")
            {
                bmr = 88.36 + (13.4 * weightKg) + (4.8 * heightCm) - (5.7 * age);
            }
            else if (gender.ToLower() == "female" || gender.ToLower() == "жінка")
            {
                bmr = 447.6 + (9.2 * weightKg) + (3.1 * heightCm) - (4.3 * age);
            }
            else
            {
                throw new ArgumentException("Невідома стать. Вкажіть 'чоловік' або 'жінка'.");
            }
            return Math.Round(bmr, 2);
        }

        // Метод 3: Розрахунок норми води в день залежно від ваги та часу тренування
        public double CalculateWaterNorm(double weightKg, int workoutMinutes)
        {
            if (weightKg <= 0 || workoutMinutes < 0)
                throw new ArgumentException("Вага має бути > 0, а час тренування не може бути від'ємним.");

            // Базова норма: 30 мл на 1 кг ваги
            double baseWaterLiters = (weightKg * 30) / 1000.0;
            
            // Додаткова вода: 500 мл (0.5 л) за кожні 30 хвилин тренування
            double extraWaterLiters = (workoutMinutes / 30.0) * 0.5;

            return Math.Round(baseWaterLiters + extraWaterLiters, 2);
        }
    }
}