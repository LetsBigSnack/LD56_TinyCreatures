namespace Data
{
    public class Toast
    {
        public NotificationType type;
        public AchievementJSON achievementJSON = null;
        public string title = "";
        public string description = "";

        public Toast(NotificationType type, AchievementJSON achievement, string title, string description)
        {
            this.type = type;
            this.achievementJSON = achievement;
            this.title = title;
            this.description = description;
        }
    }

}