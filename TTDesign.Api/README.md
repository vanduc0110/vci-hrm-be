# HRM VCI Website Api
## 1. Information
- This is project web api (BE) for ttdesign website
- Environment development: 
    - ASP.NET Core 6.0
    - Visual Studio 2022 Community
    - MySql 
- Swagger API dev [link](https://api-hrm-dev.vcijsc.com/swagger/index.html)
    - danh sách API cung cấp kèm mô tả, input/output
    - danh sách model sử dụng, kèm mô tả trường
- Diagram Flow [link](https://drive.google.com/file/d/1xrqkCJHDYvoBCH-fW7kKu_O005zXnyag/view?usp=sharing)
    - use case chung cho user staff và user admin
    - work flow cho api
- cấu trúc Database
    - cấu trúc [image.svg](/TTDesign.Api/Docs/new.svg)
    - tài liệu mô tả (cập nhật link sau)
    - mô tả tại mục #14
## 2. Config 
### [appsettings.json](/appsettings.Development.json)
- ConnectionStrings
    - VCI_DB: connection string mysql
- Mail
    - Email: mail thực hiện gửi
    - Password: password của mail send
    - SystemAdmin: mail System

### generate model context database
- require package Pomelo.EntityFrameworkCore.MySql
- run package command (from DB to DBContext)
```
Scaffold-DbContext "connection string" Pomelo.EntityFrameworkCore.MySql -OutputDir Domain/Models -Context AppDbContext -f
```
(note: [docs](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-scaffold-example.html), [comment](https://stackoverflow.com/questions/70224907/unable-to-resolve-service-for-type-microsoft-entityframeworkcore-diagnostics-idi))
- run command (from DBContext to DB)
```
add-migration dbcontext

update-database
```

## 3. Start Service (cần cập nhật lại)
- run project in [folder](/)
- swagger http://localhost:1152/swagger/index.html

## 4. Create new service (cần cập nhật lại)
- copy template file: [TempServiice](/Services/) and [ITempService](/Domain/Services/)
- define program.cs
```
builder.Services.AddScoped<ITempService, TempService>();
```
## 5. Create new Repository (cần cập nhật lại)
copy template file: [TempRepository](/Persistence/Repositories/) and [ITempRepository](/Domain/Repositories/)
- define program.cs
```
builder.Services.AddScoped<ITempRepository, TempRepository>();
```

## 6. Architecture project (cần cập nhật lại)
Folder/File |      -       | Description 
----------- | ----------- | -----------
[Constants](/Constants/) | - | define constants
/ | [EnumStatic.cs](/Constants/EnumStatic.cs) | fields const using in project
/ | [EnumStatic.cs](/Constants/MessageContents.cs) | Messages response
[Controllers](/Controllers/) | | define controllers
[Docs](/Docs/) | | documents of project
/ | [diagram.drawio](/Docs/diagram.drawio) | diagram of database
/ | [ReviewRolesByObjects.xlsx](/Docs/ReviewRolesByObjects.xlsx) | roles user
[Domain](/Domain/) | | -
/ | [Models](/Domain/Models/) | models mapping database 
/ | [Repositories](/Domain/Repositories/) | interface repository
/ | [Security](/Domain/Security/) | ...
/ | [Services](/Domain/Services/) | interface service
[Extensions](/Extensions/) | | -
/ | [ErrorDetails.cs](/Extensions/ErrorDetails.cs) | class error details
/ | [ExceptionMiddleware.cs](/Extensions/ExceptionMiddleware.cs) | global exception middleware
/ | [ModelStateExtension.cs](/Extensions/ModelStateExtension.cs) | model state validate
[Hubs](/Hubs/) | | -
[logs](/logs/) | | default place write logs
[Mapping](/Mapping/) | | -
[Persistent](/Persistent/) | | -
/ | [Contexts](/Persistent/Contexts/) | DBContext
/ | [Repositories](/Persistent/Repositories/) | class repositories
[Resources](/Resources/) | | defile model resources and responsces
[Security](/Security/) | | -
[Seeds](/Seeds/) | | define start data when start project
[Services](/Services/) | | class services
[appsettings.json](/appsettings.json) | | config settings
[Program.cs](/Program.cs) | | -

## 7. Business (cần cập nhật lại)
- hệ thống tự khởi tạo 1 user System trong db: system@ttdesignco.com/ttdesignco, không thể thay đổi
- mật khẩu mặc định của user mới: ttdesignco
- quy trình khi start project
    - khởi chạy (user system được khai báo)
    - login by system user
    - create team
    - create user into team
    - create group, create project
- 1 team có thể chưa có user nào (create/edit team sẽ ko bao gồm thông tin user) nhưng 1 user bắt buộc phải nằm trong 1 team nào đó (khi tạo user bắt buộc phải chọn team)
- bảng phân role chi tiết tại [ReviewRolesByObjects.xlsx](/Docs/ReviewRolesByObjects.xlsx)
- architect: 
    - controller chứa định nghĩa các api, xác định role, validate param, đẩy logic về services
    - services tương ứng với các object xử lý tất cả các logic, kết nối giữa controller và repository
    - repository ánh xạ từ model table bằng EF core trong database
    - OOP, controller, service, repository hướng tới hoạt động độc lập
- server có cơ chế loop mỗi ngày check 1 lần, nếu là mồng 1 của tháng thì thực hiện thêm record cho bảng leave (tương ứng thêm ngày nghỉ cho nv trong tháng mới)
- server cũng sẽ có cơ chế tự động add ngày nghỉ lễ/nghỉ đặc biệt, cố định vào timesheet của nv (system sẽ thêm ngày lịch nghỉ lễ, ngày nghỉ của cty, hệ thống hằng ngày sẽ check, nếu có lịch nghỉ mới sẽ thực hiện apply vào timesheet cho nv) => FE cần view để quản lý, lên lịch nghỉ đặc biệt

## 8. Localization (cần cập nhật lại vì có apply logic code mới gọn và tường minh hơn)
- using Localization IStringLocalizer
- includes:
    - file [ShareResource.cs](/Resources/ShareResource.cs): class ShareResource
    - files resource .resx in folder [Languages](/Language/Resources/): key - value language
    - const class LocalizationKey in [file](/Constants/MessageContent.cs): key of Resource.resx
- using:
    - add param IStringLocalizer<SharedResource> to controller
    - using IStringLocalizer<SharedResource>.GetString({key const}).Value to get value

## 9. [TempController.cs](/Controllers/TempController.cs) (cần cập nhật lại)
- include function: 
    - [GET] /api/{object}/GetOption: get list rút gọn object để làm lựa chọn cho các select box
    - [GET] /api/{object}/GetListView: get list object for View List
    - [GET] /api/{object}/GetDetail/{id}: get detail object for View Detail
    - [POST] /api/{object}/Create: create new object
    - [PUT] /api/{object}/Update: update exist object
    - [DELETE] /api/{object}/Delete: delete object
    - func ValidateModel: validate model create/update object, support for api Create/Update

## 10. Swagger
```
chưa biết viết phần này ntn
```

## 11. Nhật ký
- tản mạn khi code, tự confirm business
### Coding conversion
- các file IRepository ([IRepository.cs](/Domain/Repositories/ITempRepository.cs)), Repository ([Reposityry.cs](/Persistence/Repositories/TempRepository.cs)), IService ([IService.cs](/Domain/Services/ITempService.cs)), Service ([Service.cs](/Services/TempService.cs)), Controller ([TempController.cs](/Controllers/TempController.cs)), Resources ([TempResource.cs](/Resources/TempResource.cs)) đều có file temp sẵn, khi tạo mới chỉ cần copy ra rồi sửa theo nghiệp vụ
    - Controller: 
        - sẽ có các api temp: Create, Update, Delete, GetOption, GetListView, GetDetail và func validate: ValidateBeforeCreate, ValidateBeforeUpdate, ValidateBeforeDelete
        - các nghiệp vụ đặc trưng khác thì custom riêng
    - Service:
        - class GenericService ([GenericService.cs](/Service/GenericService.cs)) và interface IGenericService ([IGenericService.cs](/Domain/Services/IGenericService.cs)) cung cấp các func sẵn: GetById, GetAll, Exist, DeleteById
        - interface BaseServiceResource ([IGenericService.cs](/Domain/Services/IGenericService.cs)) cung cấp temp func: Create, Update, khi implement cần define
        - interface BaseServiceOption ([IGenericService.cs](/Domain/Services/IGenericService.cs)) cung cấp temp func: GetOption, khi implement cần define
        - interface BaseServiceList ([IGenericService.cs](/Domain/Services/IGenericService.cs)) cung cấp temp func: GetList, khi implement cần define
        - interface BaseServiceDetail ([IGenericService.cs](/Domain/Services/IGenericService.cs)) cung cấp temp func: GetDetail, khi implement cần define
    - Repository
        - class GenericRepository ([GenericRepository.cs](/Persistence/Repositories/GenericRepository.cs)) và interface IGenericRepository ([IGenericRepository.cs](/Domain/Repositories/IGenericRepository.cs)) cung cấp các func sẵn: CreateAsync, CreatesAsync, Update, Updates, DeleteById, Delete, Deletes, GetByIdNoTrack, GetById, GetByCondition, GetAll, Exist
    - Resource:
        - sẽ có các class temp: 
            - TempResource: model object phục vụ valid dữ liệu cho api Create/Update, bao gồm logic valid
            - TeamResponse: model object phục vụ response cho api GetListView
            - TeamViewResource: model object phục vụ response cho api GetDetail
            - TeamOption: model object phục vụ response cho api GetOption
    - Coding:
        - các trường type/status trong DB thiết kế kiểu tinyint/bit/int, còn khi trả response cho FE sẽ được format kiểu string
        - trong response trả về khoản thời gian, đơn vị giờ, FE cần hiển thị kiểu giờ:phút thì model class sẽ có 2 field kiểu double và kiểu string format tương ứng, sử dụng hàm common bên trong định nghĩa class
        - tên Enum [Tên bảng/Object] + [tên Column/field của object] + [type enum] + [(option) note]
        - các bảng type/status dùng kiểu int, phải add comment để biết các giá trị và ý nghĩa của nó
    - Commit code:
        - nội dung commit theo định dạng [type]: [mô tả]
        - [type] thì có các lựa chọn
            - feat: tính năng mới (feature)
            - fix: sửa lỗi
            - docs: cập nhật tài liệu
            - style: thêm khoảng trắng, format code, dấu phẩy...
            - refactor: đổi tên hàm, tác hàm con, xóa code thừa...
            - perf: cải thiện hiệu năng
            - test: thêm test case, sửa unit test...
            - build: thay đổi ảnh hưởng đến quá trình build
            - update: cập nhật logic
        - [mô tả]: không quá dài, sử dụng câu mệnh lệnh ở thì hiện tại, không viết hóa hay dấu kết câu
    - Other:
        - các func trong controller, interface service, interface repository phải comment giải thích rõ nghiệp vụ để phục vụ đọc hiểu code và general tài liệu cho swagger
- Localization: hiện tại hỗ trợ các ngôn ngữ: en (default), vi
    - Name của object, property được khai báo trong [DisplayNameResource.resx](/Languages/DisplayNameResource.resx)
    - Các message lỗi được khai báo trong [DisplayMessageResource.resx](/Languages/DisplayMessageResource.resx)

### các issue đã xử lý
- Code, kỹ thuật
    + [user] upload avatar
        1. đã tạo api rồi
    + [Structure] có kế thừa IGenerateService => đang phân vân 
        1. có kế thừa, xây dựng bộ interface Service
    + [Role] cấu trúc policy cũ đang chưa đáp ứng dược nhu cầu phân quyền, phải sửa lại 
        1. định nghĩa bộ claim trong hệ thống, bộ api trong controller, bộ menu và các thông tin matching với nhau
        2. đã note thông tin vào readme
    + [Database] field modifiedDate chưa được update => chưa thử được cách nào
        1. tạo query chạy update column modifiedDate
    + [Convension] System.ObjectDisposedException: 'Cannot access a disposed object. ObjectDisposed_ObjectName_Name' => phương thức async ko nên return void, mà Task
    + [Auth] có 1 chỗ khai báo match giữa danh sách claim và danh sách menu? danh sách api => thêm api Auth/GetMenuAccess
    + [User] tạo leader nhưng bảng Team ko cập nhật Leader => không tái hiện được lỗi nữa
    + [Convension] đâu là nơi quy định format datetime BE hay FE => nếu giá trị hiển thị trong bảng thì BE làm, còn các option filter trên giao diện thì FE làm
    + [Timesheet] cần có luồng nghiệp vụ tạo timesheet kéo theo khi user mới được tạo trong hệ thống => đã thêm logic system request: khi user tạo mới/chuyển trạng thái active/inactive thì sẽ có 1 request system chờ, scope background server sẽ quét hằng ngày và xử lý request này vào ngày hôm sau
    + [Timesheet] đầu tháng, background service sẽ tạo record timesheet cho tháng sau nữa, vì hệ thống đang giới hạn xin nghỉ trong 1 tháng tới (chỉ có holidays là hơi khó xem vì khi chưa tới holiday thì ko hiển thị trên giao diện, hoặc nếu xem tháng tương lai thì chỉ việc select bảng holiday thôi) => nghĩa là user sẽ có sẵn record timesheet của 2 tháng => với các tháng tương lai, trả lại dữ liệu timesheet giả chỉ từ dữ liệu holiday
    + [Timesheet] theo suy luận, bảng timesheet detail đang là bảng to (nhiều cột, nhiều record) và được query nhiều nhất, sau đó có thể OT request, Leave request, timesheet, nên những dữ liệu summary sẽ tạo 1 bảng riêng ánh xạ từ các bảng này ra nhằm phân tán query và chia nhỏ dữ liệu => không cần tạo bảng report
    + [Database] khi thực hiện chuyển dữ liệu DB cũ sang DB mới
        + cũ users.order_no ý nghĩa là gì => chuyển thành finger ID
        + cũ: ntn là 1 user active/inactive => user không nằm trong 1 team nào là user inactive
        + finger printer id trong cũ là trường nào => trong bảng timesheet có time_in time_out
    + [Timesheet] có nên cập nhật design cho timesheet theo tháng: holiday là cả 1 ngày, thì nếu holiday thì đổi màu cả ngày đó, chứ ko tạo thẻ project bên trong nữa và để phân biệt ngày hôm đó là overtime cho ngày holiday, còn holiday special là 1 kiểu leave đặc biệt => cập nhật design cho ngày holiday riêng
    + [Overtime] chưa tạo được request overnight => đã thêm logic
    + [Timesheet] khi sync database, chưa có logic tính toán lại hour trong bảng timesheet => đã bổ sung trong query
    + [Structure] ko apply được JsonIgnore => build lại project, làm dần mỗi ngày 1 ít ở nhà => đã sửa được, phần gây conflict là "AddNewtonsoftJson( opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore )"
    + [Overtime] chưa code logic check request OT vượt 40h/1 tháng và 300h/1 năm => đã có, xử lý bằng logic trong scope background, chạy đầu hằng tháng, tính toán compensatory cho tháng đó
    + [Timesheet] chưa có luồng sync dữ liệu vân tay về => đã tạo thêm app console để sync dữ liệu vân tay về db, có tính toán totalHour
    + [Timesheet] chưa có luồng sync nên chưa có luồng tính toán actual hour => đã có, từ logic sync dữ liệu vân tay về db
    + [Overtime] hệ thống cũ chưa có logic ghi nhận giờ nghỉ bù chuyển đổi từ overtime vượt quá quy định => đã xử lý khi chạy scope background
    + [Role] tạo api trả về bộ quyền của user login để phục vụ FE vẽ menu => đã tạo api /Auth/GetMenuAccess
    + [Role] có lỗi: khi cập nhật role, role bị sửa teamId nên sau khi update thành công, không thấy record hiển thị trên list => đã fix
    + [noti] chưa có tính năng gửi mail và notification => đã xây dựng
    + [BackgroundScope] thêm luồng background clear notification quá cũ => đã thêm luồng xóa
    + [other] chuyển avatar từ db cũ sang db mới không => không
    + [database] timesheet thường sẽ tạo trước 2 tháng, khi chuyển dữ liệu DB, nếu như chưa tạo đủ sẵn thì làm ntn => đã chuẩn bị sẵn câu query trong file query (3) trong docs
- BA, nghiệp vụ
    + List User, loại bỏ cột role => vì kỹ thuật lấy role name rắc rối và nghiệp vụ cũng ko cần thiết hiển thị ở danh sách này lắm 
        1. đã cập nhật design figma
    + 1 admin tạo group, thì được chỉ định group, 1 leader tạo group ko được chỉ định => khi tạo project 
        1. group ko phân theo team, tự do tạo
    + role sẽ ko phân theo Team, mà phạm vi sử dụng theo position
    + role System ko nên có full quyền, mà chỉ có quyền về mặt setup, quản lý 
        1. không được, vì cần 1 user full quyền để phân quyền xuống user còn lại
    + trong finger printer, thêm tính năng chủ động tạo timesheet + chấm công (backup cho TH user chưa được active) 
        1. okie, thêm button
    + project A đang dùng group 1 có 2 user X, Y, khi update group 1 có 2 user X, Z thì có cập nhật user làm việc cho group A ko? nếu ko thì làm cách nào để cập nhật?
        1. sửa group, kéo theo cập nhật user làm project
    + có nên đơn giản hóa request OT: chỉ request time start- time end OT, chứ ko phải điền chi tiết OT cho dự án gì, để gom hết việc khai báo chi tiết công việc vào timesheet hết
        1. không bỏ project trong overtime request
    + 1 user ko là admin, muốn xem Group/Project => sao biết user được xem hay ko => bỏ qua group, user có quyền view thì được xem mọi project trên hệ thống??? => không, user chỉ được xem theo team của mình
    + khi user thuộc team mà bị inactive => có amount lại ko, tương tự user thuộc group mà bị inactive => có remove đi ko => có update theo
    + api nào chưa có trong postman thì chưa dc test
    + khi tạo project, chưa tự động tạo Project Number => vì nghiệp vụ không phức tạp, nên phần sinh project number này làm logic đơn giản
    + nếu Overnight vào ngày lễ thì tính ntn
    + TH tạo wfh trước/sau khi tạo timesheet- finger printer sẽ tương tác xử lý ntn => không cho tạo
    + ai là người quyết định được user được phép OT => người được phân role Admin/Overtime/Approve
    + khi user bị inactive, là user nghỉ việc, sẽ thu hồi hết ngày nghỉ còn lại của user chứ => thu hồi hết các timesheet kể từ ngày sau khi thực hiện inactive
    + ý thứ 2: vậy thì khi user đó được trở lại làm việc thì coi như một user mới => thì hệ thống sẽ tạo timesheet lại cho user

### các issue chưa xử lý
- Code, kỹ thuật
    - unit test => đã khởi tạo test, chưa có UT cho authorize, UT cho ModelState.IsValid
    - tạo 1 bộ test case đầy đủ để làm temp
    - chưa test TH xóa category/object mà isUing false, timesheet detail có dữ liệu
    - kiểm tra business upload file document cho project
    - đưa config mail vào environment, biến môi trường trên con dev đang nhận là biến prod nên config swagger trong program ko dc
    - chưa thiết kế/code phần request mượn/trả thiết bị
    - luồng accept invitation chưa hoàn thiện
    - khi tạo holiday, chặn ko cho tạo vào timesheet đã lock
    - thêm các noti cảnh báo khi system request ko thể thực hiện được
    - user (phân quyền đặc biệt?) cho HR
    - chưa có logic tổng hợp timesheet cho ngày hoán đổi
- BA, nghiệp vụ
    - GM thuộc 1 team nào đó
    - xin nghỉ thai sản 6 tháng, thì auto tính từ ngày bắt đầu + 6 tháng hay là chọn ngày kết thúc ntn thì trừ như thế => sau đó khi nào nv nữ đi làm lại thì active lại chăng
    => ngày kết thúc auto, approve thì vào ngày bắt đầu, user sẽ được chuyển trạng thái sang Maternity ~ tương tự như trạng thái inactive, đến này hết thì user tự động được chuyển trạng thái active, và tạo system request active user, nếu user quay trở lại sớm hơn thì admin chỉ việc vào active để ngày hôm sau bắt đầu sinh timesheet
    => về kỹ thuật:
    1 khi leave được approve, tạo system request type Maternity
    2 tới ngày đó thì 
    - cần 1 tài liệu xử lý luồng (tài liệu luồng nghiệp vụ)

## 12. Menu reference Api
### list claim
- liệt kê các đối tượng (Object) và các claim tương ứng được định nghĩa trong hệ thống

|Object\Action|view|create|update|delete|inactive|approve|lock|holiday|report|
|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
|<strong>staff|
|dashboard|x|
|timesheet|x|
|timesheet other|x|
|overtime|x|
|leave|x|
|calendar|x|
|asset|x|
|WFH|x|
|<strong>admin|
|user|x|x|x||x|||||
|team|x|x|x|x||||||
|group|x|x|x|x||||||
|role|x|x|x|x||||||
|project|x|x|x|x||||||
|analysis|x||||||||x|
|category|x|x|x|x||||||
|object|x|x|x|x||||||
|finger printer|x||x|||||||
|overtime|x|||||x|||x|
|leave|x|||||x||x|x|
|asset|x|||||x|||x|
|WFH|x|||||x||||
### reference menu vs claim 
- cách tạo role-claim = controller + ":" + menu/tab menu + "." action 
- với role tối thiểu như dưới bảng, menu này được hiển thị để user truy cập
- dữ liệu được BE trả về thông qua api /api/Auth/GetMenuAccess

|menu\claim|tab|view|create|update|delete/inactive|approve|
|:---:|---|:---|:---|:---|:---|:---|
|<strong>staff|
|Dashboard||staff:dashboard.view|
|Timesheet||staff:timesheet.view|
|Overtime||staff:overtime.view|
|Leave Form||staff:leave.view|
|Calendar||staff:calendar.view|
|Asset||staff:asset.view|
|WFH||staff:wfh.view|
|<strong>admin|
|User|
||User|admin:user.view|admin:user.create|admin:user.update|admin:user.inactive|
||Team|admin:team.view|admin:team.create|admin:team.update|admin:team.delete|
||Group|admin:group.view|admin:group.create|admin:group.update|admin:group.delete|
||Role & Permission|admin:role.view|admin:role.create|admin:role.update|admin:role.delete|
|Timesheet|
||Project|admin:project.view|admin:project.create|admin:project.update|admin:project.delete|
||Analysis|admin:analysis.view|
||Object|admin:object.view|admin:object.create|admin:object.update|admin:object.delete|
||Category|admin:category.view|admin:category.create|admin:category.update|admin:category.delete|
||Finger Printer|admin:fingerprint.view|admin:fingerprint.create|admin:fingerprint.update|admin:fingerprint.delete|
||Report|admin:analysis.view|
|Overtime|
||Request|admin:overtime.view||||admin:overtime.approve
||Summary|admin:overtime.report|
|Leave Form|
||Request|admin:leave.view||||admin:leave.approve
||Summary|admin:leave.report|
||Holiday|admin:leave.holiday|admin:leave.holiday||admin:leave.holiday
|Asset||admin:asset.view||||admin:asset.approve|
|WFH||admin:wfh.view||||admin:wfh.approve|
### reference api vs claim 
- về cơ bản, api nào thì sẽ yêu cầu authorise require với claim tương ứng
    - api Create của controller A sẽ require authorise admin:A.create
    - api Update của controller A sẽ require authorise admin:A.update
    - api Delete của controller A sẽ require authorise admin:A.delete
    - api Approve của controller A sẽ require authorise admin:A.approve
    - api GetList của controller A sẽ require authorise admin:A.view
    - api GetDetail của controller A sẽ require authorise admin:A.view
- đặc biệt sẽ có những api dùng chung như /GetOption hoặc api đặc thù Users/ChangeStatus sẽ require claim riêng, và bảng bên dưới sẽ mô tả những TH đặc biệt đó
- chú thích:
    - mỗi ngoặc là 1 bộ claim require authorise
    - chi tiết code tại /Extensions/ServiceCollectionExtension.cs

|Controller|api|claims|
|:---:|---|---|
|Category|
||GetOption|(staff:timesheet.view)|
|Finger Printer|
||Create|(admin:fingerprint.update)
||Update|(admin:fingerprint.update)
||Delete|(admin:fingerprint.update)
|Group|
||GetOption|(admin:project.create)<br>(admin:project.update)
|Leave|
|Object|
||GetOption|(staff:timesheet.view)|
|Other|
||GetOptionPosition|(admin:user.create)<br>(admin:user.update)|
||GetOptionStatus|(admin:user.view)|
||GetOptionClient|(admin:user.create)<br>(admin:user.update)|
||GetOptionTypeProject|(admin:project.create)<br>(admin:project.update)|
||GetOptionStatusProject|(admin:project.view)<br>(admin:project.create)<br>(admin:project.update)|
||GetNewProjectNumber|(admin:project.create)|
|Overtime|
||GetDetail|(staff:overtime.view)<br>(admin:overtime.view)|
||GetRequestList|(staff:overtime.view)|
|Overtime|
||GetRequestList|(staff:leave.view)|
|WFH|
||GetRequestList|(staff:wfh.view)|
|Project|
||GetOption|(admin:analysis.view)<br>(admin:analysis.report)|
||GetOptionWorking|(staff:timesheet.view)<br>(staff:overtime.view)|
|Role|
||GetOption|(admin:user.create)<br>(admin:user.update)|
||GetCollection|(admin:role.view)<br>(admin:role.create)<br>(admin:role.update)|
|Team|
||GetOption|(admin:user.view)<br>(admin:group.view)<br>(admin:overtime.view)<br>(admin:leave.view)<br>(admin:wfh.view)<br>(admin:project.view)<br>(admin:category.view)<br>(admin:object.view)|
|User|
||GetOption|(admin:group.create)<br>(admin:group.update)<br>(admin:fingerprint.view)<br>(admin:fingerprint.update)|
||GetDynamicOption|(staff:calendar.view)<br>(staff:asset.view)<br>(admin:leave.holiday)|
||ChangeStatus|(admin:user.inactive)|
||ResetPassword|(admin:user.update)|
|Holiday|
||GetInfor|(admin:leave.holiday)|
||GetCreate|(admin:leave.holiday)|
||GetDelete|(admin:leave.holiday)|
## 13. Note
- database structure
    - field id: long
    - field code: varchar(30)
    - field name: tiny varchar(20), short varchar(100), long varchar(200)
    - field description, about, note: varchar(500)
    - field date: datetime
- Tag fields
    - [input]: field nhập
    - [not input]: field không nhập
    - [hide]: field ẩn
    - [require]: field phải có giá trị
    - [unique]: field check trùng
    - [format]: field sẽ được format
- Tag api
    - [Option]: api option combobox
    - [Personal]: personal
    - [Admin]: admin
    - [List]: api list object
    - [Detail]: api get detail
    - [Create]: api create new
    - [Update]: api update
    - [Delete]: api delete
    - [Report]: api report
    - [Other]: api khác
- Tag project:
    - [Note]: ghi chú
    - [TODO]: đang xử lý chưa hoàn thiện
    - [Valid]: đoạn code validate
## 14. Database
#### quy trình chuyển dữ liệu từ database cũ sang database mới
1. khởi tạo cấu trúc db bằng query [file.sql](/Docs/(1) ImportStructureDatabase.sql)
2. khởi tạo dữ liệu role + user demo [file.sql](/Docs/(2) DataDefine.sql)
3. chạy query matching dữ liệu [file.sql](/Docs/(3) QueryTranferDatabase.sql)

## 15. Notification chứa dữ liệu điều hướng màn hình
notification response sẽ có 3 thông tin
1. PathUrl: chứa path url của màn hình tương ứng, danh sách path url tương ứng menu của hệ thống
```
home: "/",
dashboard: "/dashboard",
timeSheet: "/timesheet",
timeSheetDetail: "/timesheet/detail",
overtime: "/overtime",
leaveForm: "/leaveform",
calendar: "/calendar/:userId",
wfh: "/wfh",
asset: "/asset",
userSetting: "/setting",
user: "/admin/user",
timeSheet: "/admin/timesheet",
overtime: "/admin/overtime",
leaveForm: "/admin/leaveform",
wfh: "/admin/wfh"
```
2. TabIndex: chứa index của tab chỉ định được chọn (ví dụ menu Admin-Overtime có 3 tab lần lượt Request, Summary, Report thì sẽ tương ứng index là 0, 1, 2)
3. ObjectId: định danh đối tượng được chỉ định hiển thị (ID của object)

Users/GetDashboardUser danh sách user đang thừa các user inactive của hệ thống cũ
Overtime/GetListView/2023/12 actualHour đang = 0 với các request approved cũ
Timesheet/GetListView/45/2023/11 totalHour = 0
FingerPrinter/GetListView lỗi khi call swagger: TypeError: Failed to execute 'fetch' on 'Window': Request with GET/HEAD method cannot have body.
