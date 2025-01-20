namespace Data
{
    public class Toast
    {
        public AchievementJSON achievementJSON = null;
        public string title = "";
        public string description = "";

        public Toast(AchievementJSON achievement, string title, string description)
        {
            this.achievementJSON = achievement;
            this.title = title;
            this.description = description;
        }
    }

}