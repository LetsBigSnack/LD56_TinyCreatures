namespace Data
{
    public class MaterialStats
    {
        public float yieldSpeed = 60f;
        public float yieldTime = 0;
        public BigDecimal yieldAmount = 0;

        public MaterialStats(float yieldSpeed, float yieldTime, BigDecimal yieldAmount)
        {
            this.yieldSpeed = yieldSpeed;
            this.yieldTime = yieldTime;
            this.yieldAmount = yieldAmount;
        }
    }
}
