CREATE PROCEDURE usp_Team_UpdateAmountOfTeam()
BEGIN
END;
DELIMITER $$
DROP PROCEDURE IF EXISTS usp_Team_UpdateAmountOfTeam;
CREATE PROCEDURE `usp_Team_UpdateAmountOfTeam`(
)
BEGIN
	UPDATE Team T
	LEFT JOIN (select tu.TeamId, count(1) as Amount from User u join TeamUser tu on tu.UserId = u.Id where u.IsActive = 1 group by tu.TeamId) as T2
	ON T.Id = T2.TeamId
	SET T.Amount = IF(T2.TeamId is null, 0, T2.Amount);
END$$
DELIMITER ;

CREATE PROCEDURE usp_Overtime_SummaryByUser()
BEGIN
END;
DELIMITER $$
DROP PROCEDURE IF EXISTS usp_Overtime_SummaryByUser;
CREATE PROCEDURE `usp_Overtime_SummaryByUser`(
	userId bigint, yearInp int, monthInp int
)
BEGIN
	Declare hadExist int default 0;
    Select count(1) from OvertimeSummary s where s.UserId = userId and s.`Month` = STR_TO_DATE(CONCAT(yearInp,'-',monthInp,'-','01'), '%Y-%m-%d') into hadExist;
    If hadExist = 0 then
		Begin
			INSERT INTO `OvertimeSummary` (`UserId`, `Month`, `HourWeekday`, `HourWeekend`, `HourOvernight`, `HourHoliday`, 
				`HourWeekdayToCompensatory`, `HourWeekendToCompensatory`, `HourOvernightToCompensatory`, `HourHolidayToCompensatory`) 
			select t.CreatedBy as `UserId`, STR_TO_DATE(CONCAT(yearInp,'-',monthInp,'-','01'), '%Y-%m-%d') as `Month`, 
				sum(if(td.Type = 0 and td.Paid = 0, td.ActualHour, 0)) as `weekday`, sum(if(td.Type = 1 and td.Paid = 0, td.ActualHour, 0)) as `weekend`, sum(if(td.Type = 2 and td.Paid = 0, td.ActualHour, 0)) as `overnight`, sum(if(td.Type = 3 and td.Paid = 0, td.ActualHour, 0)) as `holiday`,
				sum(if(td.Type = 0 and td.Paid = 1, td.ActualHour, 0)) as `weekday_`, sum(if(td.Type = 1 and td.Paid = 1, td.ActualHour, 0)) as `weekend_`, sum(if(td.Type = 2 and td.Paid = 1, td.ActualHour, 0)) as `overnight_`, sum(if(td.Type = 3 and td.Paid = 1, td.ActualHour, 0)) as `holiday_`
			from OvertimeRequest t join OvertimeRequestDetail td on t.Id = td.OvertimeRequestId
			where t.CreatedBy = userId and year(t.StartDate) = yearInp and month(t.StartDate) = monthInp and t.`Status` in (1, 3)
			group by t.CreatedBy;
		End;
	Else
		Begin
			Update `OvertimeSummary` s 
            Join (select t.CreatedBy as `UserId`, STR_TO_DATE(CONCAT(yearInp,'-',monthInp,'-','01'), '%Y-%m-%d') as `Month`, 
					sum(if(td.Type = 0 and td.Paid = 0, td.ActualHour, 0)) as `weekday`, sum(if(td.Type = 1 and td.Paid = 0, td.ActualHour, 0)) as `weekend`, sum(if(td.Type = 2 and td.Paid = 0, td.ActualHour, 0)) as `overnight`, sum(if(td.Type = 3 and td.Paid = 0, td.ActualHour, 0)) as `holiday`,
					sum(if(td.Type = 0 and td.Paid = 1, td.ActualHour, 0)) as `weekday_`, sum(if(td.Type = 1 and td.Paid = 1, td.ActualHour, 0)) as `weekend_`, sum(if(td.Type = 2 and td.Paid = 1, td.ActualHour, 0)) as `overnight_`, sum(if(td.Type = 3 and td.Paid = 1, td.ActualHour, 0)) as `holiday_`
				from OvertimeRequest t join OvertimeRequestDetail td on t.Id = td.OvertimeRequestId
				where t.CreatedBy = userId and year(t.StartDate) = yearInp and month(t.StartDate) = monthInp and t.`Status` in (1, 3)
				group by t.CreatedBy) tmp on tmp.UserId = s.userId and tmp.Month = s.Month
            Set s.`HourWeekday` = tmp.weekday, 
				s.`HourWeekend` = tmp.weekend, 
                s.`HourOvernight` = tmp.overnight, 
                s.`HourHoliday` = tmp.holiday, 
				s.`HourWeekdayToCompensatory` = tmp.weekday_, 
                s.`HourWeekendToCompensatory` = tmp.weekend_, 
                s.`HourOvernightToCompensatory` = tmp.overnight_, 
                s.`HourHolidayToCompensatory` = tmp.holiday_
			Where s.UserId = userId and year(s.`Month`) = yearInp and month(s.`Month`) = monthInp;
        End;
	End if;
END$$
DELIMITER ;

CREATE PROCEDURE usp_Product_UpdateQuantityItem()
BEGIN
END;
DELIMITER $$
DROP PROCEDURE IF EXISTS usp_Product_UpdateQuantityItem;
CREATE PROCEDURE `usp_Product_UpdateQuantityItem`(
	productId bigint
)
BEGIN
	update Product set -- 0: InStock, 1: Using, 2: OutStock
		`Quantity` = (select count(1) from ProductItem where ProductId = productId),
		`InStock` = (select count(1) from ProductItem where ProductId = productId and `Status` = 0),
		`Using` = (select count(1) from ProductItem where ProductId = productId and `Status` = 1)
		where Id = productId;
END$$
DELIMITER ;

 ALTER TABLE `FingerPrinter` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Group` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Holiday` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `LeaveRequest` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Leave` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `OvertimeRequest` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `ProductItem` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `ProductRequest` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Project` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Role` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Team` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `TimesheetDetail` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Timesheet` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `User` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `WfhRequest` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Notification` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `Bill` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 ALTER TABLE `ProductRequest` MODIFY COLUMN `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
 
-- demo data
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('SelfWedding', 'Self Wedding', 'Bản thân kết hôn', '5 ngày', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', '30 ngày kể từ ngày kết hôn', '4 tiếng', 'Đăng ký không liên tục', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('FamilyWedding', 'Family Wedding', 'Con kết hôn', '2 ngày', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', '30 ngày kể từ ngày kết hôn', '4 tiếng', 'Đăng ký không liên tục', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('FamilyBereavement', 'Family Bereavement', 'Cha mẹ đẻ/Vợ chồng mất', '5 ngày', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', '30 ngày kể từ ngày tang lễ', '4 tiếng', 'Đăng ký liên tục', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('RelativeBereavement', 'Relative Bereavement', 'Ông/bà ruột, Anh/chị/em ruột bố mẹ vợ/chồng mất anh/chị/em chồng/vợ mất', '3 ngày', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', '30 ngày kể từ ngày tang lễ', '4 tiếng', 'Đăng ký liên tục', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('SelfMaternity', 'Self Maternity', 'Bản thân nhân viên nữ sinh con', 'từ 4 - 6 tháng', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', '6 tháng sau ngày đăng ký, tính cả  thứ 7 chủ nhật và  ngày lễ', '6 tháng', 'Không giới hạn số lần', 'Nghỉ không lương, BHXH chi trả. Trước khi hết thời gian nghỉ thai sảntheo quy định,  lao động nữ có thể trở lại làm việc khi đã nghỉ ít nhất 4 tháng nhưng NLĐ phải báo trước, được người SDLĐ đồng ý và có xác nhận của  cơ sở khám bệnh, chữa bệnh có thẩm quyền về việc đi làm sớm không có hại cho sức khỏe của NLĐ. Trong TH đó họ tính là đi làm công thường');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('FamilyMaternity', 'Family Maternity', 'Nhân viên nam có vợ sinh con', 'từ 5 - 7 ngày', 'Bắt đầu từ khi chuyển lên nhân viên chính thức', 'Không giới hạn', '4 tiếng', 'Không giới hạn số lần', 'Nghỉ không lương, BHXH chi trả');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('Compensatory', 'Compensatory', 'Nghỉ bù', 'Giờ OT vượt quá giới hạn * hệ số lương OT', 'Tình từ lúc phát sinh nghỉ bù', '1 năm tính theo tháng phát sinh', '15 phút', 'Không giới hạn số lần', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('SummerVacation', 'Summer Vacation', 'Nghỉ hè', '4 ngày/năm', '01/07 hằng năm', '31/10 cùng năm', '4 tiếng', 'Không giới hạn số lần', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('Annual', 'Annual', 'Nghỉ phép', '1 ngày/tháng', 'Tính từ ngày 15 hàng tháng', '2 năm', '4 tiếng', 'Không giới hạn số lần', 'Nghỉ có lương');
INSERT INTO `LeaveInformation` (`Type`, `TypeName`, `Detail`, `LeaveDay`, `StartCondition`, `End`, `Block`, `Using`, `Note`) VALUES ('Unpaid', 'Unpaid', 'Nghỉ không lương', 'Không giới hạn', 'Không giới hạn', 'Không giới hạn', '15 phút', 'Không giới hạn số lần', 'Nghỉ không lương');

-- define data
-- import bằng file sql
-- dữ liệu client
insert into `Client` (`Id`, `Name`, `Code`) select `client_id`, trim(`client_name`), trim(`client_code`) from `ttdesigndatabase_old`.`client`;
-- dữ liệu team
insert into `Team` (`Id`, `Name`, `Code`, `Amount`, `CreatedBy`, `ModifiedBy`) 
select `team_id`, trim(`team_name`), trim(`team_code`), 0, 1, 1 from `ttdesigndatabase_old`.`teams` where `is_department` = 1 and `is_active` = 1;
-- user
update `User` dbnew inner join `ttdesigndatabase_old`.`users` dbold on dbnew.id = dbold.user_id
set dbnew.`FullName` = trim(dbold.`full_name`),
dbnew.`DateStartWork` = dbold.`date_start_work`,
-- dbnew.`IsActive` = dbold.`user_name`,
-- dbnew.`State` = dbold.`user_name`,
dbnew.`StaffId` = trim(dbold.`staff_id`),
-- dbnew.`Position` = dbold.`user_name`,
dbnew.`Avatar` = '/Upload/Images/0.png',
dbnew.`UserName` =  substring_index( trim(dbold.`email`), '@', 1),
dbnew.`NormalizedUserName` = upper(substring_index( trim(dbold.`email`), '@', 1)),
dbnew.`Email` = trim(dbold.`email`),
dbnew.`NormalizedEmail` = upper( trim(dbold.`user_name`)) where dbnew.Id > 0;
-- xóa các record user demo tạo sẵn để mapping dữ liệu
delete from `User` where UserName like 'demo%' and id > 0;
-- tạo default role
INSERT INTO `UserRoles` (`UserId`, `RoleId`) select Id, 6 from User;
-- user info
insert into `UserInfo` (`UserId`, `PhoneNumber`, `Gender`, `Birthday`, `IdNo`, `IssuedTo`, `IssuedBy`, `Address`, `SocialInsuranceBookNo`, `AboutMe`, `FingerId`) 
select user_id, trim(phone_number), trim(gender), birthday, trim(id_no), trim(issued_to), trim(issued_by), trim(address), trim(social_insurance_book_no), trim(about_me), 
`order_no` from `ttdesigndatabase_old`.`users`;
-- quan hệ team user
insert into `TeamUser` (`TeamId`, `UserId`) 
select * from (select team_id, user_id from `ttdesigndatabase_old`.`team_users` where `status` = 'active' and team_id in (1,2,3,4,5,6) group by team_id, user_id) temp;
-- cập nhật trạng thái active của user: trong DB cũ, nếu user không có quan hệ với team trên bảng team_user thì user đó inactive
update `User` u left join `TeamUser` t on u.Id = t.UserId
set u.IsActive = 0 where u.Id != 1 and t.UserId is null and u.Id > 0;
-- cập nhật số user nằm trong 1 team
UPDATE Team T
LEFT JOIN (select tu.TeamId, count(1) as Amount from User u join TeamUser tu on tu.UserId = u.Id where u.IsActive = 1 group by tu.TeamId) as T2
ON T.Id = T2.TeamId 
SET T.Amount = IF(T2.TeamId is null, 0, T2.Amount) where T.id > 0;
-- user setting
insert into `UserSetting` (`Id`, `EmailNotification`, `Timezone`, `Language`) 
select user_id, trim(email_notification), trim(timezone), trim(`language`)
from ttdesigndatabase_old.user_settings;
-- position và role thì cần làm bằng tay, khi hệ thống chạy thì cần người review và phân position + role
-- group
insert into `Group` (`Id`, `Name`, `Code`, `Amount`, `CreatedBy`, `ModifiedBy`) 
select team_id, trim(team_name), trim(team_code), 
0, 1, 1 from `ttdesigndatabase_old`.`teams` where is_department = 0 and is_active = 1;
-- tạo quan hệ group user
insert into `GroupUsers` (`GroupId`, `UserId`)
(select t.team_id, tu.user_id from ttdesigndatabase_old.teams t 
join ttdesigndatabase_old.team_users tu on t.team_id = tu.team_id and tu.`status` = 'active'
join (select id from User where IsActive = 1) users on users.id = tu.user_id
where t.is_department = 0 and t.is_active = 1);
-- cập nhật số user nằm trong 1 group
UPDATE `Group` T
LEFT JOIN (select tu.GroupId, count(1) as Amount from User u join GroupUsers tu on tu.UserId = u.Id where u.IsActive = 1 group by tu.GroupId) as T2
ON T.Id = T2.GroupId 
SET T.Amount = IF(T2.GroupId is null, 0, T2.Amount) where T.id > 0;
-- category
insert into `TeamCategory` (`Id`, `TeamId`, `Name`, `IsUsing`, `CreatedBy`) 
select timesheet_category_id, team_id, trim(`name`), case when is_active = 1 then true else false end, 1 from `ttdesigndatabase_old`.`timesheet_categories`;
-- object
insert into `TeamObject` (`Id`, `TeamId`, `Name`, `IsUsing`, `CreatedBy`) 
select timesheet_object_id, team_id, trim(`name`), case when is_active = 1 then true else false end, 1 from `ttdesigndatabase_old`.`timesheet_objects`;
-- project
-- ALTER TABLE `Project` DROP INDEX `fk_projects_code_idx`;
-- có column timesheet_projects.project_def_id = 1 (project office) và 133 (project hozen) là không có record tương ứng bảng project_def
insert into `Project` (`Id`, `TeamId`, `ProjectManagement`, `ClientId`, `StartedDate`, `FinishedDate`, `Status`, `IsPublic`, `ProjectNumber`, `QuotationHour`, 
`Type`, `Code`, `Name`, `FiscalYear`, `WorkingHour`, `OvertimeHour`, `CreatedBy`, `ModifiedBy`) 
select p.timesheet_project_id, p.team_id, 
case when p.team_id = 1 then 34 when p.team_id = 2 then 20 when p.team_id = 3 then 32 when p.team_id = 4 then 22 when p.team_id = 5 then 37 else 0 end as PM, 
pd.client_id, p.started_date, p.finished_date, 
case when p.finished_date is not null then 2 when p.is_active = 1 then 1 else 0 end as status, p.is_public, pd.project_number, 0 as quotationHour, pd.project_type, 
pd.project_code, pd.project_description, pd.fiscal_year, 0 as workingHour, 0 as overtimeHour, 1, 1
from ttdesigndatabase_old.timesheet_projects p
join ttdesigndatabase_old.project_def pd on pd.project_id = p.project_def_id;
-- sử dụng trường timesheet_projects.timesheet_project_id là project ID, mà db old đang bị khuyết project ID = 1 nên chủ động thêm vào
-- INSERT INTO `ttdesigndatabase_old`.`project_def` (`project_id`, `fiscal_year`, `project_type`, `project_number`, `team_code`, `project_code`, `project_description`, `client_id`) VALUES ('1', '01', 'T', '01', 'HR', 'TEMP', 'project template', '1');
-- UPDATE `ttdesigndatabase_old`.`timesheet_projects` SET `project_def_id` = '1' WHERE (`timesheet_project_id` = '1');
-- và project ID 39
INSERT INTO `Project` (`Id`, `TeamId`, `ProjectManagement`, `ClientId`, `StartedDate`, `Status`, `IsPublic`, `ProjectNumber`, `QuotationHour`, `Type`, `Code`, `Name`, `FiscalYear`, `WorkingHour`, `OvertimeHour`, `CreatedBy`, `ModifiedBy`) VALUES ('39', '1', '34', '2', '2022-05-12 05:00:00', '1', b'0', 21, b'0', 'T', 'NHS', 'NKE Hozen Team System', '7', b'0', b'0', b'1', b'1');
-- project + user
INSERT INTO `ProjectUsers` (`UserId`, `ProjectId`) 
select distinct tu.user_id, tp.timesheet_project_id
from ttdesigndatabase_old.team_projects tp
join ttdesigndatabase_old.timesheet_projects p on p.timesheet_project_id = tp.timesheet_project_id
join ttdesigndatabase_old.teams t on t.team_id = tp.team_id
join ttdesigndatabase_old.team_users tu on t.team_id = tu.team_id
where tu.`status` = 'active' and tu.user_id != 1 group by tp.timesheet_project_id, tu.user_id order by tp.timesheet_project_id, tu.user_id;
-- timesheet
DELIMITER $$
CREATE FUNCTION MathTimesheetHourTotal(timeIn DateTime, timeOut DateTime) returns double DETERMINISTIC
BEGIN
	Declare hours double;
	If hour(timeIn) < 8 then
		Set timeIn = STR_TO_DATE(CONCAT(year(timeIn),'-',LPAD(month(timeIn),2,'00'),'-',LPAD(day(timeIn),2,'00'),' 0800'), '%Y-%m-%d %h%i');
	End if;
    If hour(timeIn) < 12 and hour(timeOut) < 12 then
		Begin
			Set hours = 0;
		End;
	Elseif hour(timeIn) < 12 and hour(timeOut) >= 12 and hour(timeOut) < 13 then 
		Begin
			Set timeOut = DATE_ADD(STR_TO_DATE(CONCAT(year(timeIn),'-',LPAD(month(timeIn),2,'00'),'-',LPAD(day(timeIn),2,'00')), '%Y-%m-%d'), INTERVAL 12 HOUR);
			Set hours = 0;
		End;
	Elseif hour(timeIn) < 12 and hour(timeOut) >= 13 then 
		Set hours = -1;
	Elseif hour(timeIn) >= 12 and hour(timeOut) < 13 then 
		Set hours = 0;
	Elseif hour(timeIn) >= 12 and hour(timeOut) >= 13 then 
		Begin
			Set timein = DATE_ADD(STR_TO_DATE(CONCAT(year(timeIn),'-',LPAD(month(timeIn),2,'00'),'-',LPAD(day(timeIn),2,'00')), '%Y-%m-%d'), INTERVAL 13 HOUR);
			Set hours = 0;
		End;
	Elseif hour(timeIn) >= 13 then 
		Set hours = 0;
	End if;
	if minute(timediff(timeOut, timeIn))/60 < 0.25 then
		Set hours = hours + hour(timediff(timeOut, timeIn));
	elseif minute(timediff(timeOut, timeIn))/60 < 0.5 then
		Set hours = hours + hour(timediff(timeOut, timeIn)) + 0.25;
	elseif minute(timediff(timeOut, timeIn))/60 < 0.75 then
		Set hours = hours + hour(timediff(timeOut, timeIn)) + 0.5;
	else
		Set hours = hours + hour(timediff(timeOut, timeIn)) + 0.75;
	End if;
    return hours;
END$$ 
DELIMITER ;
-- bảng timesheet có user_id (53,54,55,56,57,58,59) nhưng bảng users lại ko có các user_id này
insert into `Timesheet` (`Id`, `UserId`, `Date`, `LockBy`, `HolidayName`, `CreatedBy`, `ModifiedBy`) 
select t.timesheet_id, t.user_id, t.`date`, case when t.`lock` = 0 then 1 else null end as lockby, 
(select name from ttdesigndatabase_old.public_holiday_of_year where from_date<= t.`date` and to_date>=t.`date`) as holidayname, 1 as created, 1 as modied from ttdesigndatabase_old.timesheet t
where t.user_id not in (53,54,55,56,57,58,59) and t.date >= '2023-01-01';
-- finger printer
-- do bảng timesheet có case time_in time_out null nên sẽ lấy giá trị default
insert into `FingerPrinter` (`TimesheetId`, `DateIn`, `DateOut`, `Note`, `HourTotal`, `CreatedBy`, `ModifiedBy`) 
select t.timesheet_id, if(t.time_in is null, t.`date`, t.time_in), 
case when t.time_out is not null then t.time_out when t.time_out is null and t.time_in is not null then t.time_in else t.`date` end, 
"async data", 
if(t.time_in is not null and t.time_out is not null, MathTimesheetHourTotal(t.time_in, t.time_out), 0) as hourTotal, 1, 1 from ttdesigndatabase_old.timesheet t 
where t.user_id not in (53,54,55,56,57,58,59) and t.`date` >= '2023-01-01';
-- timesheet detail
-- log task project bình thường
insert into `TimesheetDetail` (`Type`, `TimesheetId`, `ProjectId`, `TimesheetCategoryId`, `TimesheetObjectId`, `Description`, `Hours` ,
`CreatedBy` ,`ModifiedBy`, `ReferenceId`) 
select '0' as type, tt.timesheet_id, tt.timesheet_project_id, tt.timesheet_category_id, tt.timesheet_object_id, trim(tt.`description`), tt.hours, t.user_id, t.user_id, 0 as referenceid
from ttdesigndatabase_old.timesheet t 
join ttdesigndatabase_old.timesheet_tasks tt on t.timesheet_id = tt.timesheet_id
where t.`date` >= '2023-01-01';
-- overtime request
-- tạo thêm bộ object + category TranferData cho các record overtime (chỉ làm 1 lần với db old)
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('1', 'TranferData', b'1', b'0', '1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('2', 'TranferData', b'1', b'0', '1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('3', 'TranferData', b'1', b'0', '1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('4', 'TranferData', b'1', b'0', '1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('5', 'TranferData', b'1', b'0', '1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_categories` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('6', 'TranferData', b'1', b'0', '1');

-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('1', 'TranferData', b'1', b'0', b'1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('2', 'TranferData', b'1', b'0', b'1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('3', 'TranferData', b'1', b'0', b'1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('4', 'TranferData', b'1', b'0', b'1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('5', 'TranferData', b'1', b'0', b'1');
-- INSERT INTO `ttdesigndatabase_old`.`timesheet_objects` (`team_id`, `name`, `is_active`, `is_public`, `created_by`) VALUES ('6', 'TranferData', b'1', b'0', b'1');

insert into `OvertimeRequest` (`Id`, `StartDate`, `EndDate`, `Reason`, `ProjectId`, `CategoryId`, `ObjectId`, `Status`, `Reviewer`,
`CreatedBy` ,`ModifiedBy`) 
select o.overtime_id, o.from_time, o.to_time,trim(o.reason), o.timesheet_project_id, u.timesheet_category_id as cate, u.timesheet_object_id as obj,
case when o.status_definition_id = 1 then 0 when o.status_definition_id = 2 then 1 when o.status_definition_id = 3 then 2 else 0 end as `status`,
if(o.modified_by is not null, o.modified_by, null) as reviewer, o.user_id, if(o.modified_by is not null, o.modified_by, o.user_id)
from ttdesigndatabase_old.timesheet t
join ttdesigndatabase_old.overtimes o on t.timesheet_id = o.timesheet_id
join (select u.user_id, tc.timesheet_category_id, tos.timesheet_object_id 
from ttdesigndatabase_old.users u 
join ttdesigndatabase_old.team_users tu on u.user_id = tu.user_id
join ttdesigndatabase_old.teams t on tu.team_id = t.team_id and t.is_department = 1
join ttdesigndatabase_old.timesheet_categories tc on tc.team_id = t.team_id and tc.`name` = 'TranferData'
join ttdesigndatabase_old.timesheet_objects tos on tos.team_id = t.team_id and tos.`name` = 'TranferData' 
group by u.user_id, tc.timesheet_category_id, tos.timesheet_object_id) u on u.user_id = t.user_id
where t.date >= '2023-01-01';
-- overtime request detail
insert into `OvertimeRequestDetail` (`OvertimeRequestId`, `Type`, `Paid`, `ActualHour`, `Multiplier`, `Start`, `End`, `CreatedBy`) 
select o.overtime_id, case when o.overtime_type_id = 4 then 3 when o.overtime_type_id = 2 then 1 when o.overtime_type_id = 3 then 2 else 0 end as `type`, 
0 as paid, o.actual_hours, 
case when o.overtime_type_id = 4 then 3 when o.overtime_type_id = 2 then 2 when o.overtime_type_id = 3 then 2.1 else 1.5 end as `multiplier`, o.from_time, o.to_time, o.user_id
from ttdesigndatabase_old.timesheet t
join ttdesigndatabase_old.overtimes o on t.timesheet_id = o.timesheet_id
where t.date >= '2023-01-01' and o.status_definition_id = 2;
-- overtime summary
insert into `OvertimeSummary` (`UserId`, `Month`, `HourWeekday`, `HourWeekend`, `HourOvernight`, `HourHoliday`, 
`HourWeekdayToCompensatory`, `HourWeekendToCompensatory`, `HourOvernightToCompensatory`, `HourHolidayToCompensatory`) 
select user_id, STR_TO_DATE(CONCAT(_year,'-',LPAD(_month,2,'00'),'-','01'), '%Y-%m-%d') as `month`,
sum(if(overtime_type_id = 1,sum_hours,0)), sum(if(overtime_type_id = 2,sum_hours,0)), sum(if(overtime_type_id = 3,sum_hours,0)), sum(if(overtime_type_id = 4,sum_hours,0)),
0, 0, 0, 0
from (
select o.user_id, month(from_time) as _month, year(from_time) as _year, overtime_type_id, sum(o.actual_hours) as sum_hours
from ttdesigndatabase_old.timesheet t
join ttdesigndatabase_old.overtimes o on t.timesheet_id = o.timesheet_id
where t.date >= '2023-01-01' and o.status_definition_id = 2 group by o.user_id, month(from_time), year(from_time), overtime_type_id) tmp 
group by user_id, STR_TO_DATE(CONCAT(_year,'-',LPAD(_month,2,'00'),'-','01'), '%Y-%m-%d');
-- overtime rule
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('0', '0', '8', '22', 'weekday', '1.5', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('0', '1', '8', '22', 'weekday', '1.5', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('0', '2', '8', '22', 'weekday', '1.5', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('0', '3', '8', '22', 'weekday', '1.5', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('0', '4', '8', '22', 'weekday', '1.5', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('1', '5', '8', '22', 'weekend', '2', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('1', '6', '8', '22', 'weekend', '2', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '0', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '1', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '2', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '3', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '4', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '5', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '6', '0', '8', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '0', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '1', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '2', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '3', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '4', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '5', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('2', '6', '22', '24', 'overnight', '2.1', '1');
INSERT INTO `OvertimeRule` (`Type`, `Weekday`, `HourStart`, `HourEnd`, `Description`, `Multiplier`, `CreatedBy`) VALUES ('3', null, 0, 24, 'holiday', '3', '1');
-- timesheet từ request overtime
insert into `TimesheetDetail` (`Type`, `TimesheetId`, `ProjectId`, `TimesheetCategoryId`, `TimesheetObjectId`, `Description`, `Hours` ,
`CreatedBy` ,`ModifiedBy`, `ReferenceId`) 
-- select case when o.overtime_type_id = 4 then 4 when o.overtime_type_id = 2 then 2 when o.overtime_type_id = 3 then 3 else 1 end as `type`, 
select 1 as `type`, 
t.timesheet_id, o.timesheet_project_id, u.timesheet_category_id as cate, u.timesheet_object_id as obj, 
trim(o.reason), o.actual_hours, o.user_id, o.user_id, o.overtime_id
from ttdesigndatabase_old.timesheet t
join ttdesigndatabase_old.overtimes o on t.timesheet_id = o.timesheet_id
join (select u.user_id, tc.timesheet_category_id, tos.timesheet_object_id 
from ttdesigndatabase_old.users u 
join ttdesigndatabase_old.team_users tu on u.user_id = tu.user_id
join ttdesigndatabase_old.teams t on tu.team_id = t.team_id and t.is_department = 1
join ttdesigndatabase_old.timesheet_categories tc on tc.team_id = t.team_id and tc.name = 'TranferData'
join ttdesigndatabase_old.timesheet_objects tos on tos.team_id = t.team_id and tos.name = 'TranferData' 
group by u.user_id, tc.timesheet_category_id, tos.timesheet_object_id) u on u.user_id = t.user_id
where t.`date` >= '2023-01-01' and o.status_definition_id = 2;
-- leave request
insert into `LeaveRequest` (`Id`, `Type`, `StartDate`, `EndDate`, `Reason`, `Hour`, `Status`, `Reviewer`,
`CreatedBy` ,`ModifiedBy`) 
select l.leaveform_id, 
case when l.leave_type_id = 1 then '8' when l.leave_type_id = 3 then '7' when l.leave_type_id = 4 then '0' 
when l.leave_type_id = 5 then '4' when l.leave_type_id = 6 then '9' when l.leave_type_id = 7 then '6' else 0 end as `type`, 
l.from_date, l.to_date, l.reason, l.hours, 
case when l.status_definition_id = 1 then 0 when l.status_definition_id = 2 then 1 when l.status_definition_id = 3 then 2 else 0 end as `status`, 
if(l.modified_by is not null, l.modified_by, null) as reviewer, l.created_by, if(l.modified_by is not null, l.modified_by, l.user_id)
from ttdesigndatabase_old.leaveform l where l.from_date > '2023-01-01';
-- leave request detail
insert into `LeaveRequestDetail` (`LeaveRequestId`, `Date`, `Hours`) 
select l.leaveform_id, date(l.from_date), l.hours
from ttdesigndatabase_old.leaveform_detail l where l.from_date > '2023-01-01';
-- timesheet từ request leave
insert into `TimesheetDetail` (`Type`, `TimesheetId`, `Description`, `ProjectId`, `Hours`, `CreatedBy` ,`ModifiedBy`, `ReferenceId`)
select case when l.leave_type_id = 6 then 2 else 3 end as `type`, -- cần phân biệt type 2 UnpaidLeave, 3 PaidLeave
(select Id from Timesheet where UserId = l.user_id and Date = date(ld.from_date)), 
trim(l.reason), case when l.leave_type_id = 6 then -1 else 0 end as `projectId`, ld.hours, l.user_id, l.user_id, l.leaveform_id
from ttdesigndatabase_old.leaveform l 
join ttdesigndatabase_old.leaveform_detail ld on l.leaveform_id = ld.leaveform_id
where l.from_date > '2023-01-01' and l.status_definition_id = 2;
-- leave 
-- leave history 
DELIMITER $$
CREATE PROCEDURE CreateAnnualLeave (IN userCode varchar(20), IN dayRemainLeave double, IN firstAnnualLeave double, IN dayAnnual2023 int, IN daySummer2023 int) 
BEGIN
	Declare userId bigint;
	Select Id from User where StaffId = userCode into userId; 
    -- tạo leave cho khởi tạo dữ liệu bắt đầu năm 2023
    If firstAnnualLeave > 0 then
		INSERT INTO `Leave` (`UserId`, `Type`, `Date`, `Hours`, `Using`, `CreatedBy`, `ModifiedBy`) 
		values (userId, '8', DATE_ADD("2023-01-01", INTERVAL -dayRemainLeave MONTH), '8', 8-firstAnnualLeave*8, '1', '1');
		INSERT INTO `LeaveHistory` (`Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
		values ('0', concat('Add Annual of month ', DATE_FORMAT(DATE_ADD("2023-01-01", INTERVAL -dayRemainLeave-1 MONTH), '%Y-%m')), firstAnnualLeave, '0', '0', firstAnnualLeave*8, userId, '2023-01-01');
	End if;
    While dayRemainLeave > 0 do
		INSERT INTO `Leave` (`UserId`, `Type`, `Date`, `Hours`, `Using`, `CreatedBy`, `ModifiedBy`) 
        values (userId, '8', DATE_ADD("2023-02-01", INTERVAL -dayRemainLeave MONTH), '8', '0', '1', '1');
		INSERT INTO `LeaveHistory` (`Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
		values ('0', concat('Add Annual of month ', DATE_FORMAT(DATE_ADD("2023-01-01", INTERVAL -dayRemainLeave MONTH), '%Y-%m')), firstAnnualLeave+1, '0', '0', '8', userId, '2023-01-01');
        set dayRemainLeave = dayRemainLeave - 1;
        set firstAnnualLeave = firstAnnualLeave + 1;
    End while;
    -- add leave năm 2023
    While dayAnnual2023 > 0 do
		INSERT INTO `Leave` (`UserId`, `Type`, `Date`, `Hours`, `Using`, `CreatedBy`, `ModifiedBy`) 
        values (userId, '8', DATE_ADD("2024-02-01", INTERVAL -dayAnnual2023 MONTH), '8', '0', '1', '1');
		INSERT INTO `LeaveHistory` (`Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
		values ('0', concat('Add Annual of month ', DATE_FORMAT(DATE_ADD("2024-01-01", INTERVAL -dayAnnual2023 MONTH), '%Y-%m')), firstAnnualLeave+1, '0', '0', '8', userId, '2023-01-01');
        set dayAnnual2023 = dayAnnual2023 - 1;
        set firstAnnualLeave = firstAnnualLeave + 1;
    End while;
    -- add leave năm 2024
	INSERT INTO `Leave` (`UserId`, `Type`, `Date`, `Hours`, `Using`, `CreatedBy`, `ModifiedBy`) 
	values 
    (userId, '8', "2024-02-01", '8', '0', '1', '1'),
    (userId, '8', "2024-03-01", '8', '0', '1', '1'),
    (userId, '8', "2024-04-01", '8', '0', '1', '1'),
    (userId, '8', "2024-05-01", '8', '0', '1', '1'),
    (userId, '8', "2024-06-01", '8', '0', '1', '1');
	INSERT INTO `LeaveHistory` (`Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
	values 
    ('0', 'Add Annual of month 2024-01', firstAnnualLeave+1, '0', '0', '8', userId, '2024-01-01'),
    ('0', 'Add Annual of month 2024-02', firstAnnualLeave+2, '0', '0', '8', userId, '2024-01-01'),
    ('0', 'Add Annual of month 2024-03', firstAnnualLeave+3, '0', '0', '8', userId, '2024-01-01'),
    ('0', 'Add Annual of month 2024-04', firstAnnualLeave+4, '0', '0', '8', userId, '2024-01-01'),
    ('0', 'Add Annual of month 2024-05', firstAnnualLeave+5, '0', '0', '8', userId, '2024-01-01');
    set firstAnnualLeave = firstAnnualLeave + 5;
    -- add summer leave năm 2023
    If daySummer2023 > 0 then
		INSERT INTO `Leave` (`UserId`, `Type`, `Date`, `Hours`, `Using`, `CreatedBy`, `ModifiedBy`) 
        values (userId, '7', '2023-07-01', daySummer2023*8, '0', '1', '1');
		INSERT INTO `LeaveHistory` (`Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
		values ('1', 'Add SummerVacation of year 2023', firstAnnualLeave, daySummer2023, '0', daySummer2023*8, userId, '2023-01-01');
    End if;
END$$ 
DELIMITER ;
-- NV0041	Nguyễn Thị Huyền Trang
CALL CreateAnnualLeave('NV0041', 8, 0.5, 12, 4);
-- NV0043	Hoàng Thị Lý
CALL CreateAnnualLeave('NV0043', 10, 0, 12, 4);
-- NV0052	Nguyễn Quốc Hưng
CALL CreateAnnualLeave('NV0052', 2, 0.5, 12, 4);
-- NV0054	Phạm Công Hoan
CALL CreateAnnualLeave('NV0054', 1, 0, 12, 4);
-- NV0057	Vũ Văn Khu
CALL CreateAnnualLeave('NV0057', 2, 0.5, 12, 4);
-- NV0058	Hoàng Sơn Lâm
CALL CreateAnnualLeave('NV0058', 1, 0, 12, 4);
-- NV0060	Đào Văn Vang
CALL CreateAnnualLeave('NV0060', 1, 0, 12, 4);
-- NV0064	Nguyễn Thế Hà
CALL CreateAnnualLeave('NV0064', 1, 0, 12, 4);
-- NV0066	Vương Đình Toản
CALL CreateAnnualLeave('NV0066', 6, 0, 12, 4);
-- NV0068	Đinh Quang Tuấn
CALL CreateAnnualLeave('NV0068', 7, 0, 12, 4);
-- NV0070	Nguyễn Thảo Nguyên
CALL CreateAnnualLeave('NV0070', 2, 0, 12, 4);
-- NV0074	Nguyễn Văn Đức
CALL CreateAnnualLeave('NV0074', 4, 0, 12, 4);
-- NV0075	Nguyễn Văn Trí
CALL CreateAnnualLeave('NV0075', 1, 0, 12, 4);
-- NV0076	Nguyễn Sĩ Hiệp
CALL CreateAnnualLeave('NV0076', 3, 0.5, 12, 4);
-- NV0078	Dương Văn Phúc
CALL CreateAnnualLeave('NV0078', 1, 0.5, 12, 4);
-- NV0079	Bùi Hoàng Nguyên
CALL CreateAnnualLeave('NV0079', 1, 0.5, 12, 4);
-- NV0080	Phan Văn Huy
CALL CreateAnnualLeave('NV0080', 2, 0, 12, 4);
-- NV0083	Phạm Khương Duy
CALL CreateAnnualLeave('NV0083', 1, 0, 12, 4);
-- NV0084	Đỗ Đức Quang
CALL CreateAnnualLeave('NV0084', 0, 0, 11, 4);
-- NV0085	Lâm Trọng Đạt
CALL CreateAnnualLeave('NV0085', 0, 0, 12, 4);
-- NV0086	Nguyễn Đức An
CALL CreateAnnualLeave('NV0086', 0, 0, 9, 4);
-- NV0087	Dương Tất Minh
CALL CreateAnnualLeave('NV0087', 0, 0, 9, 4);
-- NV0088	Trinh Thi Thu Trang
CALL CreateAnnualLeave('NV0088', 0, 0, 9, 4);
-- NV0089	Trần Văn Khương
CALL CreateAnnualLeave('NV0089', 0, 0, 9, 4);
-- NV0090	Ưng Bình Sơn
CALL CreateAnnualLeave('NV0090', 0, 0, 6, 4);
-- NV0091	Nguyễn Xuân Thắng
Update User set StaffId = 'NV0091' where Id = 90;
CALL CreateAnnualLeave('NV0091', 0, 0, 6, 4);
-- NV0092	Trần Phương Nam
CALL CreateAnnualLeave('NV0092', 0, 0, 4, 0);
-- holiday
insert into `Holiday` (`Name`, `StartDate`, `EndDate`, `Type`, `Status`, `CreatedBy` ,`ModifiedBy`) 
select `name`, from_date, to_date, b'0' as `type`, 1 as `status`, 1, 1
from ttdesigndatabase_old.public_holiday_of_year where from_date >= '2023-01-01';
-- tạo dữ liệu sử dụng leave và leave history từ leave request
DELIMITER $$
CREATE PROCEDURE ApplyLeaveRequest (IN userId bigint)
BEGIN
	Declare finished integer default 0;
	-- 
	Declare leaveRequestId bigint;
    -- thông tin của leave request
	Declare leaveType integer;
	Declare hourLeave double;
    Declare startDate datetime;
    Declare endDate datetime;
    Declare leaveId bigint;
    Declare hourTotal double;
    Declare hourUsing double;
    -- thông tin leave history
    Declare annualLeave double;
    Declare summerLeave double;
    Declare compensatoryLeave double;
    Declare descriptionDefine varchar(100);
    -- lấy leave request id bắt đầu và kết thúc để chạy loop
    Declare arrayLeaveRequestId cursor for select l.Id from LeaveRequest l where l.Status = 1 and l.CreatedBy = userId and l.Type in (8, 7); -- 7 SummerVacation, 8 Annual
    Declare continue handler for not found set finished = 1;
    
    Open arrayLeaveRequestId;
    loopRequest: Loop
		Fetch arrayLeaveRequestId into leaveRequestId;
        If finished = 1 then 
			leave loopRequest;
		End if;
        Select l.`Type`, l.`StartDate`, l.`EndDate`, l.`Hour`, concat('Use ', 
			case when Type = 0 then 'SelfWedding' when Type = 1 then 'FamilyWedding' when Type = 2 then 'FamilyBereavement' when Type = 3 then 'RelativeBereavement' 
            when Type = 4 then 'SelfMaternity' when Type = 5 then 'FamilyMaternity' when Type = 6 then 'Compensatory' when Type = 7 then 'SummerVacation'
            when Type = 8 then 'Annual' else 'Unpaid' end, 
			' from ', DATE_FORMAT(l.StartDate, '%Y-%m-%d %H:%i'), ' to ', DATE_FORMAT(l.EndDate, '%Y-%m-%d %H:%i')) 
			from `LeaveRequest` l where l.Id = leaveRequestId into leaveType, startDate, endDate, hourLeave, descriptionDefine;
        -- get old history
		Select l.AnnualLeave, l.SummerLeave, l.CompensatoryLeave from LeaveHistory l where l.CreatedBy = userId order by l.id desc limit 1 into annualLeave, summerLeave, compensatoryLeave;
		If annualLeave is null then
			Set annualLeave = 0;
		End if;
		If summerLeave is null then
			Set summerLeave = 0;
		End if;
		If compensatoryLeave is null then
			Set compensatoryLeave = 0;
		End if;
        
		-- add history
		If leaveType = 8 then
			INSERT INTO `LeaveHistory` (`LeaveRequestId`, `Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
			select leaveRequestId, '6', descriptionDefine, annualLeave-hourLeave/8, summerLeave, compensatoryLeave, 0-hourLeave, userId, CONCAT(DATE_FORMAT(startDate, '%Y-%m-%d'));
		Elseif leaveType = 7 then
			INSERT INTO `LeaveHistory` (`LeaveRequestId`, `Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
			select leaveRequestId, '7', descriptionDefine, annualLeave, summerLeave-(hourLeave/8), compensatoryLeave, 0-hourLeave, userId, CONCAT(DATE_FORMAT(startDate, '%Y-%m-%d'));
		 Elseif leaveType = 6 then
			INSERT INTO `LeaveHistory` (`LeaveRequestId`, `Type`, `Description`, `AnnualLeave`, `SummerLeave`, `CompensatoryLeave`, `Unit`, `CreatedBy`, `CreatedDate`) 
			select leaveRequestId, '8', descriptionDefine, annualLeave, summerLeave, compensatoryLeave-(hourLeave/8), 0-hourLeave, userId, CONCAT(DATE_FORMAT(startDate, '%Y-%m-%d'));
		End if;
        While hourLeave > 0 do -- nếu có tìm thấy leave request id, lần lượt lấy Leave còn sử dụng được, tương ứng với số giờ trong leave request
			-- lấy record Leave cũ nhất còn sử dụng được
			Select l.`Id`, l.`Hours`, l.`Using` from `Leave` l
				where l.`UserId` = userId and l.`Type` = leaveType and l.`Hours` > l.`Using` order by l.`Id` limit 1 into leaveId, hourTotal, hourUsing;
			If hourTotal - hourUsing > hourLeave then
				-- using leave
				Update `Leave` set `Using` = hourUsing + hourLeave where Id = leaveId;
				-- add leave history using
				INSERT INTO `LeaveHistoryUsing` (`LeaveId`, `LeaveRequestId`, `Hours`)
				values (leaveId, leaveRequestId, hourLeave);
				Set hourLeave = 0;
			Else 
				-- using leave
				Update `Leave` set `Using` = hourTotal where Id = leaveId;
				-- add leave history using
				INSERT INTO `LeaveHistoryUsing` (`LeaveId`, `LeaveRequestId`, `Hours`)
				values (leaveId, leaveRequestId, hourTotal - hourUsing);
				Set hourLeave = hourLeave - (hourTotal - hourUsing);
			End if;
		End while;
    End loop loopRequest;
    Close arrayLeaveRequestId;
END$$ 
DELIMITER ;
call ApplyLeaveRequest(16);
call ApplyLeaveRequest(22);
call ApplyLeaveRequest(23);
call ApplyLeaveRequest(32);
call ApplyLeaveRequest(34);
call ApplyLeaveRequest(37);
call ApplyLeaveRequest(38);
call ApplyLeaveRequest(39);
call ApplyLeaveRequest(42);
call ApplyLeaveRequest(43);
call ApplyLeaveRequest(45);
call ApplyLeaveRequest(47);
call ApplyLeaveRequest(60);
call ApplyLeaveRequest(74);
call ApplyLeaveRequest(78);
call ApplyLeaveRequest(80);
call ApplyLeaveRequest(81);
call ApplyLeaveRequest(82);
call ApplyLeaveRequest(83);
call ApplyLeaveRequest(84);
call ApplyLeaveRequest(85);
call ApplyLeaveRequest(86);
call ApplyLeaveRequest(87);
call ApplyLeaveRequest(88);
call ApplyLeaveRequest(89);
call ApplyLeaveRequest(90);
call ApplyLeaveRequest(91);
-- khởi tạo system request cho tạo timesheet user

-- timesheet report
-- sau khi transfer đủ dữ liệu timesheet, overtime, leave thì viết query tổng hợp
insert into `TimesheetReport` (`TimesheetId`, `ProjectId`, `Hours` , `OvertimeHours`)
select TimesheetId, ProjectId, sum(if(Type <> 1, Hours, 0)) as sum, sum(if(`Type` = 1, Hours, 0)) as OT
from TimesheetDetail
group by TimesheetId, ProjectId;
-- khởi tạo role cho user system
UPDATE `UserRoles` SET `RoleId` = '1' WHERE `UserId` = '1';
UPDATE `UserRoles` SET `RoleId` = '2' WHERE `UserId` = '6';
UPDATE `UserRoles` SET `RoleId` = '3' WHERE `UserId` = '20';
UPDATE `UserRoles` SET `RoleId` = '3' WHERE `UserId` = '22';
UPDATE `UserRoles` SET `RoleId` = '3' WHERE `UserId` = '32';
UPDATE `UserRoles` SET `RoleId` = '3' WHERE `UserId` = '34';
UPDATE `UserRoles` SET `RoleId` = '3' WHERE `UserId` = '37';
UPDATE `User` SET `Position` = '0' WHERE (`Id` = '1');
UPDATE `User` SET `Position` = '1' WHERE (`Id` = '6');
UPDATE `User` SET `Position` = '2' WHERE (`Id` = '20');
UPDATE `User` SET `Position` = '2' WHERE (`Id` = '22');
UPDATE `User` SET `Position` = '2' WHERE (`Id` = '32');
UPDATE `User` SET `Position` = '2' WHERE (`Id` = '34');
UPDATE `User` SET `Position` = '2' WHERE (`Id` = '37');
INSERT INTO UserClaims (`UserId`, `ClaimType`, `ClaimValue`) VALUES ('1', 'admin:user', 'edit'),('6', 'admin:user', 'edit'),('20', 'admin:user', 'edit'),('22', 'admin:user', 'edit'),('32', 'admin:user', 'edit'),('34', 'admin:user', 'edit'),('37', 'admin:user', 'edit');
DELETE FROM `TeamUser` WHERE (`UserId` = '1') and (`TeamId` = '4');

DROP PROCEDURE IF EXISTS CreateAnnualLeave;
DROP PROCEDURE IF EXISTS ApplyLeaveRequest;
DROP FUNCTION IF EXISTS MathTimesheetHourTotal;

INSERT INTO `CalendarObject` (`Type`, `Name`)
VALUES ('0', 'Notice'), ('1', 'Meeting Room 1'), ('2', 'HiEnd PC 1');

-- tạo request tạo timesheet cho user active
-- INSERT INTO `SystemRequest` (`UserId`, `Type`, `Date`, `Status`, `ObjectId`) VALUES ('0', 'DefineTimesheetNextMonth', '2024-06-01', '0', '0');

-- tạo request tạo timesheet cho từng user active
-- INSERT INTO `SystemRequest` (`UserId`, `Type`, `Date`, `Status`, `CreatedBy`) -- VALUES ('1', '0', '2024-04-01', '0', '1')
-- select u.Id, 0 as Type, DATE_ADD(tmp.StartDate, INTERVAL 1 DAY), 0 as Status, 1 as CreatedBy from User u 
-- left join (select UserId, max(Date) as StartDate from Timesheet group by UserId) tmp on tmp.UserId = u.Id
-- where u.IsActive = 1 and u.Id not in (1, 50, 79, 49, 26, 27, 20, 48);

-- unlock timesheet để test
-- update Timesheet set LockBy = null where id > 0;

-- sửa holiday EndDate do dữ liệu khởi tạo sai
UPDATE `Holiday` SET `EndDate` = '2024-04-30 00:00:00' WHERE (`Id` = '8');

-- do id finger lệch giữa dữ liệu chạy thực tế và dữ liệu db nên cần sửa lại trực tiếp
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '76' WHERE (`UserId` = '16');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '43' WHERE (`UserId` = '23');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '432' WHERE (`UserId` = '24');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '9999' WHERE (`UserId` = '76');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '9998' WHERE (`UserId` = '77');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '79' WHERE (`UserId` = '78');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '77' WHERE (`UserId` = '79');
UPDATE `QuangDemo`.`UserInfo` SET `FingerId` = '78' WHERE (`UserId` = '81');