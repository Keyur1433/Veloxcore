select * from Bookings 
select * from Activities 
select * from Users

select b.BookingDate, a.Name, a.Price, b.BookingId, a.Id as ActivityId, a.SafetyLevel from Bookings b
join Activities a 
on b.ActivityId = a.Id
-- where a.SafetyLevel = 'low'
-- where a.Name = 'Laser Tag'
where b.BookingDate between '2025-10-01' and '2025-10-17'

truncate table Users
truncate table Activities
truncate table Bookings

go
create or alter procedure sp_GetBookings
@CustomerName varchar(50) = null,
@ActivityName varchar(50) = null,
@SafetyLevel varchar(30) = null,
--@MinPrice decimal(5,2) = null,
--@MaxPrice decimal(5,2) = null,
@BookingDateFrom datetime = null,
@BookingDateTo datetime = null
--@TimeSlotType varchar(30) = null
as
begin
	select b.BookingId as BookingId,
	a.Id as ActivityId,
	u.Id as CustomerId,
	u.Name as CustomerName,
	a.Name as ActivityName,
	a.Price as Price,
	b.BookingDate,
	b.TimeSlot,
	b.Participants,
	u.BirthDate as CustomerBirthDate
	from Bookings b
	join Activities a
	on b.ActivityId = a.Id
	join Users u 
	on b.CustomerId = u.Id
	where (@CustomerName is null or u.Name like '%' + @CustomerName + '%')
	and (@ActivityName is null or a.Name like '%' + @ActivityName + '%')
	and (@SafetyLevel is null or a.SafetyLevel like '%' + @SafetyLevel + '%')
	and (@BookingDateFrom is null or b.BookingDate >= @BookingDateFrom)
	and (@BookingDateTo is null or b.BookingDate <= @BookingDateTo)
end