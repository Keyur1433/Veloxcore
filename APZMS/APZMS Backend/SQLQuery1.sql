select * from Bookings where BookingId = 16
select * from Activities where Id = 2
select * from Users where Id = 1003

delete from Users where Name = 'Keyur5'

select b.BookingDate, a.Name, a.Price, b.BookingId, a.Id as ActivityId, a.SafetyLevel from Bookings b
join Activities a 
on b.ActivityId = a.Id
--where a.SafetyLevel = 'medium' and
where a.Name = 'Mini Golf'
order by b.BookingDate desc
--b.BookingDate between '2025-10-10' and '2025-10-16'

truncate table Users
truncate table Activities
truncate table Bookings

go
create or alter procedure sp_GetBookings
@PageNumber int = null,
@PageSize int = null,
@CustomerName varchar(50) = null,
@ActivityName varchar(50) = null,
@SafetyLevel varchar(30) = null,
@BookingDateFrom datetime = null,
@BookingDateTo datetime = null
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
	order by b.BookingDate desc
	offset (case when @PageNumber is null or @PageSize is null 
				then 0
				else (@PageNumber -1) * @PageSize end) rows
	fetch next (case when @PageSize is null 
				then 10 
				else @PageSize end) rows only;
--OFFSET = how many rows to skip from the top of the result.
--FETCH NEXT = how many rows to return after skipping.
end