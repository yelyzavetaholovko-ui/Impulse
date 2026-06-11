using System;
using Xunit;
using Impulse.Metrics;

namespace Impulse.Tests
{
    public class ProgressTrackerTests
    {
        // Паттерн тестування AAA: Arrange (Підготовка), Act (Дія), Assert (Перевірка)

        [Fact] // TC01: Позитивний сценарій (валідні дані для відсотка ваги)
        public void CalculateWeightLossPercentage_ValidData_ReturnsCorrectPercentage()
        {
            // Arrange
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 72.0 };

            // Act
            var result = tracker.CalculateWeightLossPercentage(80.0, user);

            // Assert
            Assert.Equal(10.0, result);
        }

        [Fact] // TC02: Вага користувача дорівнює нулю (BVA, нижня межа)
        public void CalculateWeightLossPercentage_WeightZero_ThrowsArgumentException()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 0.0 };

            Assert.Throws<ArgumentException>(() => tracker.CalculateWeightLossPercentage(80.0, user));
        }

        [Fact] // TC03: Поточна вага більша за стартову (EP, невалідний клас)
        public void CalculateWeightLossPercentage_CurrentWeightHigher_ThrowsArgumentException()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 85.0 };

            Assert.Throws<ArgumentException>(() => tracker.CalculateWeightLossPercentage(80.0, user));
        }

        [Fact] // TC04: Розрахунок днів, коли мета вже досягнута (BVA, нижня межа)
        public void CalculateDaysToGoal_GoalAlreadyAchieved_ReturnsZero()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 70.0, TargetWeight = 70.0 };

            var result = tracker.CalculateDaysToGoal(user, 3500);

            Assert.Equal(0, result);
        }

        [Fact] // TC05: Валідний позитивний розрахунок днів до мети (EP)
        public void CalculateDaysToGoal_ValidData_ReturnsCorrectDays()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 75.0, TargetWeight = 70.0 }; // Треба скинути 5 кг (38500 ккал)

            var result = tracker.CalculateDaysToGoal(user, 3500); // 500 ккал/день => 77 днів

            Assert.Equal(77, result);
        }

        [Fact] // TC06: Дефіцит калорій дорівнює нулю (BVA, межа дефіциту)
        public void CalculateDaysToGoal_DeficitZero_ThrowsArgumentException()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 75.0, TargetWeight = 70.0 };

            Assert.Throws<ArgumentException>(() => tracker.CalculateDaysToGoal(user, 0));
        }

        [Fact] // TC07: Кількість пройдених тижнів менша або дорівнює нулю (BVA)
        public void UpdateGoalStatus_WeeksZeroOrNegative_ThrowsArgumentException()
        {
            var tracker = new ProgressTracker();
            var user = new User();

            Assert.Throws<ArgumentException>(() => tracker.UpdateGoalStatus(user, 0, 2.0));
        }

        [Fact] // TC08: Чудовий прогрес (схуднення за планом або швидше) (EP)
        public void UpdateGoalStatus_FastProgress_ReturnsExcellentStatus()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 75.0, TargetWeight = 70.0 };

            var result = tracker.UpdateGoalStatus(user, 4, 3.0); // 3 кг за 4 тижні = 0.75 кг/тиждень (норма >= 0.5)

            Assert.Contains("Чудовий прогрес", result);
        }

        [Fact] // TC09: Повільний прогрес, спрацьовує адаптивний ШІ-цикл (EP)
        public void UpdateGoalStatus_SlowProgress_TriggersAdaptiveLoop()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 72.0, TargetWeight = 70.0 };

            var result = tracker.UpdateGoalStatus(user, 4, 1.0); // 1 кг за 4 тижні = 0.25 кг/тиждень (повільно)

            Assert.Contains("Прогрес повільний, терміни зміщено", result);
        }

        [Fact] // TC10: Критична зупинка ваги, вихід із циклу за лімітом безпеки (BVA)
        public void UpdateGoalStatus_NoProgress_ReturnsCriticalStatus()
        {
            var tracker = new ProgressTracker();
            var user = new User { CurrentWeight = 75.0, TargetWeight = 70.0 };

            var result = tracker.UpdateGoalStatus(user, 4, 0.0); // Вага стоїть на місці, цикл дійде до ліміту в 52 тижні

            Assert.Contains("Критично повільний прогрес", result);
        }
    }
}
