using System;

namespace WindowsForms_LogBook
{
    public class Warrior : Person
    {
        public Warrior(string fullName, DateTime lastTrainingTime, string state, string examinationWorkGrade, string uFCWorkGrade, int crystalCount, string commentFromTrainer)
        {
            FullName = fullName;
            LastTrainingTime = lastTrainingTime;
            State = state;
            ExaminationWorkGrade = examinationWorkGrade;
            UFCWorkGrade = uFCWorkGrade;
            CrystalCount = crystalCount;
            CommentFromTrainer = commentFromTrainer;
        }

        public string FullName { get; set; }
        public DateTime LastTrainingTime { get; set; }
        public string State { get; set; } = AttentionStates.None;

        public string ExaminationWorkGrade { get; set; }
        public string UFCWorkGrade { get; set; }
        public int CrystalCount { get; set; } = default;

        public string CommentFromTrainer { get; set; } = default;

    }
}
