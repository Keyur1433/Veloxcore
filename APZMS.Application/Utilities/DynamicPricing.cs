namespace APZMS.Application.Common
{
    public class DynamicPricing
    {
        private decimal GetFinalTimeSlotPrice(string timeSlot, decimal slotPrice)
        {
            if (TimeSlotTypeGenerator.GetTimeSlotType(timeSlot) == "Off-Peak")
            {
                return slotPrice;
            }
            else if (TimeSlotTypeGenerator.GetTimeSlotType(timeSlot) == "Standard")
            {
                return slotPrice + slotPrice * 0.15m;
            }
            else if (TimeSlotTypeGenerator.GetTimeSlotType(timeSlot) == "Peak")
            {
                return slotPrice + slotPrice * 0.30m;
            }
            else
                return 0;
        }

        public decimal GetFinalPrice(DateTime dob, string timeSlot, decimal slotPrice)
        {
            decimal finalPrice = 0;
            if (AgeGroupHelper.GetAgeGroup(dob) == "toddler")
            {
                return finalPrice = GetFinalTimeSlotPrice(timeSlot, slotPrice) * 0.80m;
            }
            else if (AgeGroupHelper.GetAgeGroup(dob) == "kid")
            {
                return finalPrice = GetFinalTimeSlotPrice(timeSlot, slotPrice) * 0.90m;
            }
            else if (AgeGroupHelper.GetAgeGroup(dob) == "teen")
            {
                return finalPrice = GetFinalTimeSlotPrice(timeSlot, slotPrice) * 0.95m;
            }
            else if (AgeGroupHelper.GetAgeGroup(dob) == "adult")
            {
                return finalPrice = GetFinalTimeSlotPrice(timeSlot, slotPrice);
            }
            else
                return 0;
        }
    }
}
