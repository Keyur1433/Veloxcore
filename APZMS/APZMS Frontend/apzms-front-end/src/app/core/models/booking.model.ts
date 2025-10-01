export interface BookingDto {
    customerId: number;
    activityId: number;
    bookingDate: string; // ISO date
    timeSlot: string;
    participants: number;
}

export interface BookingResponseDto {
    bookingId: number;
    id: number; // Alias for bookingId if needed
    activityId: number;
    activityName: string;
    customerId: number;
    customerName: string;
    finalPrice: number;
    bookingDate: string;
    timeSlot: string;
    participants: number;
    status: string;
    timeSlotType?: string | null;
    ageGroup?: string | null;
    message?: string | null;
}

export interface BookingFilteredItemResponseDto {
    id: number;
    activityId: number;
    customerId: number;
    customerName: string;
    activityName: string;
    price: number;
    finalPrice: number;
    bookingDate: string;
    timeSlot: string;
    participants: number;
    timeSlotType: string;
}

export interface BookingPatchDto {
    participants?: number
}

export interface BookingUpdateDto {
    customerId: number
    activityId: number
    bookingDate: string
    participants: number
}

export interface BookingRequestDto {
    id: number;
    customerId: number;
    activityId: number;
    bookingDate: string;
    participants: number;
    timeSlot: string;
    status: string;
}

export interface PaginatedBookings {
  data: BookingFilteredItemResponseDto[];
  total: number;
  page: number;
  pageSize: number;
  totalPages?: number;
}