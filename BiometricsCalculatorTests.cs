using System;
using Xunit;

namespace ImpulseApp.Tests
{
    public class BiometricsCalculatorTests
    {
        // Тести для методу: CalculateBMI

        [Fact]
        public void CalculateBMI_NormalData_ReturnsCorrectBMI()
        {
            // Техніка: EP (позитивний) - TC-01
            // Arrange (Підготовка)
            var calculator = new BiometricsCalculator();
            double weight = 70;
            double height = 175;

            // Act (Виконання)
            double result = calculator.CalculateBMI(weight, height);

            // Assert (Перевірка)
            Assert.Equal(22.86, result); 
        }

        [Fact]
        public void CalculateBMI_ZeroWeight_ThrowsArgumentException()
        {
            // Техніка: BVA (негативний) - TC-02
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 0; // Граничне значення
            double height = 175;

            // Act & Assert (Для винятків дія і перевірка об'єднуються)
            Assert.Throws<ArgumentException>(() => calculator.CalculateBMI(weight, height));
        }

        [Fact]
        public void CalculateBMI_ZeroHeight_ThrowsArgumentException()
        {
            // Техніка: BVA (негативний) - TC-03
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 70;
            double height = 0; // Граничне значення

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateBMI(weight, height));
        }

        [Fact]
        public void CalculateBMI_NegativeWeight_ThrowsArgumentException()
        {
            // Техніка: EP (негативний) - TC-04
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = -5; // Недопустимий клас
            double height = 175;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateBMI(weight, height));
        }

        [Fact]
        public void CalculateBMI_NegativeHeight_ThrowsArgumentException()
        {
            // Техніка: EP (негативний) - TC-05
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 70;
            double height = -175; // Недопустимий клас

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateBMI(weight, height));
        }

        // Тести для методу: CalculateDailyCalories

        [Fact]
        public void CalculateDailyCalories_Male_ReturnsCorrectCalories()
        {
            // Техніка: EP (позитивний) - TC-06
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 75;
            double height = 180;
            int age = 25;
            string gender = "чоловік";

            // Act
            double result = calculator.CalculateDailyCalories(weight, height, age, gender);

            // Assert
            Assert.Equal(1814.86, result);
        }

        [Fact]
        public void CalculateDailyCalories_Female_ReturnsCorrectCalories()
        {
            // Техніка: EP (позитивний) - TC-07
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 60;
            double height = 165;
            int age = 30;
            string gender = "жінка";

            // Act
            double result = calculator.CalculateDailyCalories(weight, height, age, gender);

            // Assert
            Assert.Equal(1382.10, result);
        }

        [Fact]
        public void CalculateDailyCalories_UnknownGender_ThrowsArgumentException()
        {
            // Техніка: EP (негативний) - TC-08
            // Arrange
            var calculator = new BiometricsCalculator();
            string gender = "кіт"; // Недопустимий клас еквівалентності

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateDailyCalories(70, 170, 25, gender));
        }

        [Fact]
        public void CalculateDailyCalories_ZeroAge_ThrowsArgumentException()
        {
            // Техніка: BVA (негативний) - TC-09
            // Arrange
            var calculator = new BiometricsCalculator();
            int age = 0; // Граничне значення

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateDailyCalories(70, 170, age, "жінка"));
        }

        [Fact]
        public void CalculateDailyCalories_ZeroWeight_ThrowsArgumentException()
        {
            // Техніка: BVA (негативний) - TC-10
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 0; // Граничне значення

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateDailyCalories(weight, 170, 25, "чоловік"));
        }

        // Тести для методу: CalculateWaterNorm

        [Fact]
        public void CalculateWaterNorm_NoWorkout_ReturnsBaseNorm()
        {
            // Техніка: BVA (позитивний) - TC-11
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 60;
            int workoutMinutes = 0; // Нижня межа допустимих значень

            // Act
            double result = calculator.CalculateWaterNorm(weight, workoutMinutes);

            // Assert
            Assert.Equal(1.80, result);
        }

        [Fact]
        public void CalculateWaterNorm_30MinWorkout_ReturnsCorrectNorm()
        {
            // Техніка: EP (позитивний) - TC-12
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 60;
            int workoutMinutes = 30;

            // Act
            double result = calculator.CalculateWaterNorm(weight, workoutMinutes);

            // Assert
            Assert.Equal(2.30, result);
        }

        [Fact]
        public void CalculateWaterNorm_45MinWorkout_ReturnsCorrectNorm()
        {
            // Техніка: EP (позитивний) - TC-13
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 60;
            int workoutMinutes = 45;

            // Act
            double result = calculator.CalculateWaterNorm(weight, workoutMinutes);

            // Assert
            Assert.Equal(2.55, result);
        }

        [Fact]
        public void CalculateWaterNorm_ZeroWeight_ThrowsArgumentException()
        {
            // Техніка: BVA (негативний) - TC-14
            // Arrange
            var calculator = new BiometricsCalculator();
            double weight = 0; // Граничне значення

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateWaterNorm(weight, 30));
        }

        [Fact]
        public void CalculateWaterNorm_NegativeWorkout_ThrowsArgumentException()
        {
            // Техніка: EP (негативний) - TC-15
            // Arrange
            var calculator = new BiometricsCalculator();
            int workoutMinutes = -15; // Недопустимий клас

            // Act & Assert
            Assert.Throws<ArgumentException>(() => calculator.CalculateWaterNorm(60, workoutMinutes));
        }
    }
}