namespace Data
{
    public class MaterialStats
    {
        public MaterialType type;
        public float yieldSpeed = 60f;
        public int yieldTime = 0;
        public BigDecimal yieldAmount = 0;

        public MaterialStats(MaterialType type, float yieldSpeed, int yieldTime, BigDecimal yieldAmount)
        {
            this.type = type;
            this.yieldSpeed = yieldSpeed;
            this.yieldTime = yieldTime;
            this.yieldAmount = yieldAmount;
        }
    }
}
