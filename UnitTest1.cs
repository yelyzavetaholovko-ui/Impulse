using System;
using Xunit;
using Impulse.Logic;

namespace Impulse.Tests
{
    public class WorkoutPlannerTests
    {
        private readonly WorkoutPlanner _planner = new WorkoutPlanner();

        // TC-01
        [Fact]
        public void CalculateCalories_WeightIsZero_ThrowsException()
        {
            // Arrange
            double weight = 0;
            int duration = 30;
            string activity = "біг";

            // Act & Assert
            // Техніка: BVA (Межа: <= 0), негативний
            Assert.Throws<ArgumentException>(() => _planner.CalculateCalories(activity, weight, duration));
        }

        // TC-02
        [Fact]
        public void CalculateCalories_ValidData_ReturnsCorrectValue()
        {
            // Arrange
            double weight = 70;
            int duration = 30;
            string activity = "біг";

            // Act
            double result = _planner.CalculateCalories(activity, weight, duration);

            // Assert
            // Техніка: EP (Позитивний)
            Assert.Equal(294.0, result);
        }

        // TC-03
        [Fact]
        public void CalculateCalories_DurationIsZero_ThrowsException()
        {
            // Arrange
            double weight = 60;
            int duration = 0;
            string activity = "плавання";

            // Act & Assert
            // Техніка: BVA (Межа: <= 0), негативний
            Assert.Throws<ArgumentException>(() => _planner.CalculateCalories(activity, weight, duration));
        }

        // TC-04
        [Fact]
        public void CalculateCalories_DefaultActivity_ReturnsCorrectValue()
        {
            // Arrange
            double weight = 55;
            int duration = 60;
            string activity = "йога";

            // Act
            double result = _planner.CalculateCalories(activity, weight, duration);

            // Assert
            // Техніка: EP (Позитивний, default значення)
            Assert.Equal(173.2, result);
        }

        // TC-05
        [Fact]
        public void GetIntensityLevel_AgeUnderLimit_ThrowsException()
        {
            // Arrange
            int age = 9;
            int hr = 100;

            // Act & Assert
            // Техніка: BVA (Межа: < 10), негативний
            Assert.Throws<ArgumentException>(() => _planner.GetIntensityLevel(hr, age));
        }

        // TC-06
        [Fact]
        public void GetIntensityLevel_Anaerobic_ReturnsHigh()
        {
            // Arrange
            int age = 20;
            int hr = 170;

            // Act
            string result = _planner.GetIntensityLevel(hr, age);

            // Assert
            // Техніка: BVA (Межа: рівно 0.85), позитивний
            Assert.Equal("Висока (Анаеробна)", result);
        }

        // TC-07
        [Fact]
        public void GetIntensityLevel_Cardio_ReturnsMedium()
        {
            // Arrange
            int age = 20;
            int hr = 140;

            // Act
            string result = _planner.GetIntensityLevel(hr, age);

            // Assert
            // Техніка: BVA (Межа: рівно 0.70), позитивний
            Assert.Equal("Середня (Кардіо)", result);
        }

        // TC-08
        [Fact]
        public void GetIntensityLevel_FatBurn_ReturnsLow()
        {
            // Arrange
            int age = 20;
            int hr = 100;

            // Act
            string result = _planner.GetIntensityLevel(hr, age);

            // Assert
            // Техніка: BVA (Межа: рівно 0.50), позитивний
            Assert.Equal("Низька (Жироспалювання)", result);
        }

        // TC-09
        [Fact]
        public void GetIntensityLevel_Warmup_ReturnsWarmup()
        {
            // Arrange
            int age = 20;
            int hr = 80;

            // Act
            string result = _planner.GetIntensityLevel(hr, age);

            // Assert
            // Техніка: EP (Позитивний: < 0.50)
            Assert.Equal("Розминка", result);
        }

        // TC-10
        [Fact]
        public void GetGoalProgress_TargetIsNegative_ThrowsException()
        {
            // Arrange
            double current = 100;
            double target = -50;

            // Act & Assert
            // Техніка: BVA (Межа: < 0), негативний
            Assert.Throws<ArgumentException>(() => _planner.GetGoalProgress(current, target));
        }

        // TC-11
        [Fact]
        public void GetGoalProgress_TargetIsZero_ReturnsZero()
        {
            // Arrange
            double current = 100;
            double target = 0;

            // Act
            double result = _planner.GetGoalProgress(current, target);

            // Assert
            // Техніка: BVA (Межа: рівно 0, захист від ділення на нуль)
            Assert.Equal(0, result);
        }

        // TC-12
        [Fact]
        public void GetGoalProgress_ValidProgress_ReturnsPercentage()
        {
            // Arrange
            double current = 200;
            double target = 500;

            // Act
            double result = _planner.GetGoalProgress(current, target);

            // Assert
            // Техніка: EP (Позитивний)
            Assert.Equal(40.0, result);
        }
    }
}