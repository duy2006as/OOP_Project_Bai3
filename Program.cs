// Mã số sinh viên: 202418887
// Họ và tên: Hoàng Đình Duy

using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    private string _id = string.Empty;
    private string _fullName = string.Empty;
    private double _baseSalary;

    public string Id 
    { 
        get => _id; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Mã nhân sự không được rỗng.");
            _id = value;
        }
    }

    public string FullName 
    { 
        get => _fullName; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Họ tên không được rỗng.");
            _fullName = value;
        }
    }

    public double BaseSalary 
    { 
        get => _baseSalary; 
        set 
        {
            if (value < 0)
                throw new ArgumentException("Lương cơ bản không được âm.");
            _baseSalary = value;
        }
    }

    public Employee()
    {
        Id = "UNKNOWN";
        FullName = "Unnamed employee";
        BaseSalary = 0;
    }

    public Employee(string id, string fullName)
    {
        Id = id;
        FullName = fullName;
        BaseSalary = 0;
    }

    public Employee(string id, string fullName, double baseSalary)
    {
        Id = id;
        FullName = fullName;
        BaseSalary = baseSalary;
    }

    public void increaseSalary(double amount)
    {
        if (amount <= 0) throw new ArgumentException("Giá trị tăng phải dương.");
        BaseSalary += amount;
    }

    public void increaseSalary(double value, bool byPercentage)
    {
        if (value <= 0) throw new ArgumentException("Giá trị tăng phải dương.");
        if (byPercentage)
        {
            BaseSalary += BaseSalary * (value / 100.0);
        }
        else
        {
            BaseSalary += value;
        }
    }

    public virtual double calculateMonthlyCost()
    {
        return BaseSalary;
    }

    public virtual void displayInfo()
    {
        Console.WriteLine($"[Employee] Mã NS: {Id}, Họ tên: {FullName}, Lương CB: {BaseSalary}");
    }

    ~Employee()
    {
        Console.WriteLine($"[Destructor] Employee {Id} đang bị hủy.");
    }
}

public class SoftwareEngineer : Employee
{
    private string _primaryLanguage = string.Empty;
    private double _technicalAllowance;

    public string PrimaryLanguage 
    { 
        get => _primaryLanguage; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ngôn ngữ chính không được rỗng.");
            _primaryLanguage = value;
        }
    }

    public double TechnicalAllowance 
    { 
        get => _technicalAllowance; 
        set 
        {
            if (value < 0)
                throw new ArgumentException("Phụ cấp không được âm.");
            _technicalAllowance = value;
        }
    }

    public SoftwareEngineer(string id, string fullName, string primaryLanguage) 
        : base(id, fullName)
    {
        PrimaryLanguage = primaryLanguage;
        TechnicalAllowance = 0;
    }

    public SoftwareEngineer(string id, string fullName, double baseSalary, string primaryLanguage, double technicalAllowance) 
        : base(id, fullName, baseSalary)
    {
        PrimaryLanguage = primaryLanguage;
        TechnicalAllowance = technicalAllowance;
    }

    public override double calculateMonthlyCost()
    {
        return BaseSalary + TechnicalAllowance;
    }

    public override void displayInfo()
    {
        Console.WriteLine($"[SoftwareEngineer] Mã NS: {Id}, Họ tên: {FullName}, Lương CB: {BaseSalary}, Ngôn ngữ: {PrimaryLanguage}, Phụ cấp: {TechnicalAllowance}");
    }

    ~SoftwareEngineer()
    {
        Console.WriteLine($"[Destructor] SoftwareEngineer {Id} đang bị hủy.");
    }
}

public class ProjectTeam
{
    private string _projectCode = string.Empty;
    private string _projectName = string.Empty;

    public string ProjectCode 
    { 
        get => _projectCode; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Mã dự án không được rỗng.");
            _projectCode = value;
        }
    }
    public string ProjectName 
    { 
        get => _projectName; 
        set 
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Tên dự án không được rỗng.");
            _projectName = value;
        }
    }

    private Employee? _leader;
    private List<Employee> _members;

    public Employee? Leader => _leader;

    public ProjectTeam(string projectCode, string projectName)
    {
        ProjectCode = projectCode;
        ProjectName = projectName;
        _leader = null;
        _members = new List<Employee>();
    }

    public ProjectTeam(string projectCode, string projectName, Employee leader) : this(projectCode, projectName)
    {
        if (leader == null) throw new ArgumentNullException(nameof(leader), "Trưởng nhóm không được null.");
        changeLeader(leader);
    }

    public bool contains(string employeeId)
    {
        return _members.Any(m => m.Id == employeeId);
    }

    public bool addMember(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee), "Nhân sự không được null.");
        if (contains(employee.Id))
        {
            Console.WriteLine($"Nhân sự {employee.Id} đã có trong nhóm.");
            return false;
        }
        _members.Add(employee);
        return true;
    }

    public bool addMember(Employee employee, bool makeLeader)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee), "Nhân sự không được null.");
        
        if (!contains(employee.Id))
        {
            _members.Add(employee);
        }
        else if (!makeLeader)
        {
            Console.WriteLine($"Nhân sự {employee.Id} đã có trong nhóm.");
            return false;
        }

        if (makeLeader)
        {
            _leader = employee;
        }

        return true;
    }

    public bool removeMember(string employeeId)
    {
        if (_leader != null && _leader.Id == employeeId)
        {
            Console.WriteLine($"Không được xóa trưởng nhóm {employeeId} khi chưa chọn trưởng nhóm thay thế.");
            return false;
        }

        var member = _members.FirstOrDefault(m => m.Id == employeeId);
        if (member != null)
        {
            _members.Remove(member);
            return true;
        }
        return false;
    }

    public bool changeLeader(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee), "Nhân sự không được null.");

        if (!contains(employee.Id))
        {
            _members.Add(employee);
        }
        _leader = employee;
        return true;
    }

    public double calculateTotalMonthlyCost()
    {
        return _members.Sum(m => m.calculateMonthlyCost());
    }

    public void displayTeam()
    {
        Console.WriteLine($"\n--- Project Team: {ProjectName} ({ProjectCode}) ---");
        Console.WriteLine($"Trưởng nhóm: {(_leader != null ? _leader.FullName : "Chưa có")}");
        Console.WriteLine($"Tổng số thành viên: {_members.Count}");
        Console.WriteLine("Danh sách thành viên:");
        foreach (var member in _members)
        {
            member.displayInfo();
        }
        Console.WriteLine("-------------------------------------------------");
    }

    ~ProjectTeam()
    {
        _members.Clear();
        Console.WriteLine($"[Destructor] ProjectTeam {ProjectCode} đang bị hủy. Cấu trúc danh sách đã được giải phóng nhưng không hủy Employee bên trong.");
    }
}

public class Program
{
    static Employee? emp1;
    static Employee? emp2;
    static SoftwareEngineer? se1;
    static SoftwareEngineer? se2;
    static ProjectTeam? team1;

    public static void Main()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\n================= MENU KIỂM THỬ ==================");
            Console.WriteLine("1.  Tạo 2 Employee (2 constructor khác nhau)");
            Console.WriteLine("2.  Tạo 2 SoftwareEngineer (2 constructor khác nhau)");
            Console.WriteLine("3.  Tăng lương nhân sự (số tiền cố định)");
            Console.WriteLine("4.  Tăng lương nhân sự (theo %)");
            Console.WriteLine("5.  Tạo nhóm dự án không có trưởng nhóm");
            Console.WriteLine("6.  Thêm nhân sự vào nhóm");
            Console.WriteLine("7.  Thêm kỹ sư làm trưởng nhóm");
            Console.WriteLine("8.  Thử thêm lại thành viên đã tồn tại");
            Console.WriteLine("9.  Hiển thị danh sách nhóm (kiểm tra đa hình)");
            Console.WriteLine("10. Tính tổng chi phí nhân sự hằng tháng");
            Console.WriteLine("11. Thử xóa trưởng nhóm hiện tại (sẽ bị từ chối)");
            Console.WriteLine("12. Đổi trưởng nhóm rồi xóa cựu trưởng nhóm");
            Console.WriteLine("13. Tạo nhóm 2 và chia sẻ thành viên từ nhóm 1");
            Console.WriteLine("14. Hủy nhóm 2 (mô phỏng kết thúc khối lệnh)");
            Console.WriteLine("15. Kiểm tra nhân sự sau khi nhóm 2 bị hủy");
            Console.WriteLine("0.  Thoát chương trình");
            Console.WriteLine("==================================================");
            Console.Write("Mời bạn chọn chức năng (0-15): ");
            
            string choice = Console.ReadLine();
            Console.WriteLine();
            
            try
            {
                switch (choice)
                {
                    case "1": RunStep1(); break;
                    case "2": RunStep2(); break;
                    case "3": RunStep3(); break;
                    case "4": RunStep4(); break;
                    case "5": RunStep5(); break;
                    case "6": RunStep6(); break;
                    case "7": RunStep7(); break;
                    case "8": RunStep8(); break;
                    case "9": RunStep9(); break;
                    case "10": RunStep10(); break;
                    case "11": RunStep11(); break;
                    case "12": RunStep12(); break;
                    case "13": RunStep13(); break;
                    case "14": RunStep14(); break;
                    case "15": RunStep15(); break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Đã thoát chương trình.");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[LỖI] {ex.Message}");
            }

            if (choice != "0")
            {
                Console.WriteLine("\nNhấn Enter để quay lại Menu...");
                Console.ReadLine();
            }
        }
    }

    static string GetInput(string prompt, string defaultValue = "")
    {
        Console.Write($"{prompt} {(string.IsNullOrEmpty(defaultValue) ? "" : $"[{defaultValue}] ")}: ");
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input!;
    }

    static void RunStep1()
    {
        Console.WriteLine("--- BƯỚC 1: TẠO 2 EMPLOYEE ---");
        Console.WriteLine("* Employee 1 (Constructor 2 tham số - Lương mặc định 0):");
        string id1 = GetInput("Nhập Mã NS 1", "E001");
        string name1 = GetInput("Nhập Họ tên 1", "Nguyen Van A");
        emp1 = new Employee(id1, name1);
        emp1.displayInfo();

        Console.WriteLine("\n* Employee 2 (Constructor 3 tham số):");
        string id2 = GetInput("Nhập Mã NS 2", "E002");
        string name2 = GetInput("Nhập Họ tên 2", "Tran Thi B");
        double salary2 = double.Parse(GetInput("Nhập Lương cơ bản", "1500"));
        emp2 = new Employee(id2, name2, salary2);
        emp2.displayInfo();
    }

    static void RunStep2()
    {
        Console.WriteLine("--- BƯỚC 2: TẠO 2 SOFTWARE ENGINEER ---");
        Console.WriteLine("* Kỹ sư 1 (Constructor 3 tham số):");
        string id1 = GetInput("Nhập Mã KS 1", "SE001");
        string name1 = GetInput("Nhập Họ tên 1", "Le Van C");
        string lang1 = GetInput("Nhập Ngôn ngữ chính 1", "C#");
        se1 = new SoftwareEngineer(id1, name1, lang1);
        se1.displayInfo();

        Console.WriteLine("\n* Kỹ sư 2 (Constructor 5 tham số):");
        string id2 = GetInput("Nhập Mã KS 2", "SE002");
        string name2 = GetInput("Nhập Họ tên 2", "Pham Thi D");
        double salary2 = double.Parse(GetInput("Nhập Lương cơ bản", "2000"));
        string lang2 = GetInput("Nhập Ngôn ngữ chính 2", "Java");
        double allow2 = double.Parse(GetInput("Nhập Phụ cấp", "500"));
        se2 = new SoftwareEngineer(id2, name2, salary2, lang2, allow2);
        se2.displayInfo();
    }

    static void RunStep3()
    {
        if (emp1 == null) { Console.WriteLine("Vui lòng chạy Bước 1 trước để khởi tạo Employee!"); return; }
        Console.WriteLine("--- BƯỚC 3: TĂNG LƯƠNG CỐ ĐỊNH ---");
        emp1.displayInfo();
        double amount = double.Parse(GetInput("Nhập số tiền muốn tăng cho nhân sự này", "200"));
        emp1.increaseSalary(amount);
        Console.WriteLine("Sau khi tăng:");
        emp1.displayInfo();
    }

    static void RunStep4()
    {
        if (se2 == null) { Console.WriteLine("Vui lòng chạy Bước 2 trước để khởi tạo Kỹ sư!"); return; }
        Console.WriteLine("--- BƯỚC 4: TĂNG LƯƠNG THEO % ---");
        se2.displayInfo();
        double percent = double.Parse(GetInput("Nhập % muốn tăng", "10"));
        se2.increaseSalary(percent, true);
        Console.WriteLine("Sau khi tăng:");
        se2.displayInfo();
    }

    static void RunStep5()
    {
        Console.WriteLine("--- BƯỚC 5: TẠO NHÓM DỰ ÁN ---");
        string code = GetInput("Nhập mã dự án", "P001");
        string name = GetInput("Nhập tên dự án", "Dự án Alpha");
        team1 = new ProjectTeam(code, name);
        Console.WriteLine("Đã tạo nhóm thành công:");
        team1.displayTeam();
    }

    static void RunStep6()
    {
        if (team1 == null || emp1 == null) { Console.WriteLine("Vui lòng chạy Bước 1 và Bước 5 trước!"); return; }
        Console.WriteLine("--- BƯỚC 6: THÊM NHÂN SỰ VÀO NHÓM ---");
        Console.WriteLine($"Đang thêm {emp1.FullName} vào nhóm {team1.ProjectName}...");
        if (team1.addMember(emp1))
            Console.WriteLine("Thêm thành công!");
    }

    static void RunStep7()
    {
        if (team1 == null || se1 == null) { Console.WriteLine("Vui lòng chạy Bước 2 và Bước 5 trước!"); return; }
        Console.WriteLine("--- BƯỚC 7: THÊM KỸ SƯ VÀ ĐẶT LÀM TRƯỞNG NHÓM ---");
        Console.WriteLine($"Đang thêm {se1.FullName} vào nhóm {team1.ProjectName} làm trưởng nhóm...");
        if (team1.addMember(se1, true))
            Console.WriteLine("Thêm và cấp quyền trưởng nhóm thành công!");
    }

    static void RunStep8()
    {
        if (team1 == null || emp1 == null) { Console.WriteLine("Vui lòng chạy Bước 1 và 5 trước!"); return; }
        Console.WriteLine("--- BƯỚC 8: THỬ THÊM LẠI THÀNH VIÊN ĐÃ TỒN TẠI ---");
        Console.WriteLine($"Đang thử thêm lại {emp1.FullName} (Mã: {emp1.Id})...");
        team1.addMember(emp1);
    }

    static void RunStep9()
    {
        if (team1 == null) { Console.WriteLine("Vui lòng chạy Bước 5 trước!"); return; }
        Console.WriteLine("--- BƯỚC 9: HIỂN THỊ DANH SÁCH NHÓM ---");
        team1.displayTeam();
    }

    static void RunStep10()
    {
        if (team1 == null) { Console.WriteLine("Vui lòng chạy Bước 5 trước!"); return; }
        Console.WriteLine("--- BƯỚC 10: TÍNH TỔNG CHI PHÍ NHÂN SỰ ---");
        Console.WriteLine($"Tổng chi phí hằng tháng của nhóm {team1.ProjectName}: {team1.calculateTotalMonthlyCost()}");
    }

    static void RunStep11()
    {
        if (team1 == null || team1.Leader == null) { Console.WriteLine("Vui lòng chạy Bước 5 và 7 trước (Cần có trưởng nhóm)!"); return; }
        Console.WriteLine("--- BƯỚC 11: THỬ XÓA TRƯỞNG NHÓM HIỆN TẠI ---");
        Console.WriteLine($"Đang thử xóa trưởng nhóm {team1.Leader.FullName}...");
        team1.removeMember(team1.Leader.Id);
    }

    static void RunStep12()
    {
        if (team1 == null || se2 == null || team1.Leader == null) { Console.WriteLine("Vui lòng chạy đủ Bước 2, 5, 7 trước!"); return; }
        Console.WriteLine("--- BƯỚC 12: ĐỔI TRƯỞNG NHÓM RỒI XÓA CỰU TRƯỞNG NHÓM ---");
        string oldLeaderId = team1.Leader.Id;
        Console.WriteLine($"1. Đổi trưởng nhóm sang {se2.FullName}...");
        team1.changeLeader(se2);
        Console.WriteLine($"2. Xóa cựu trưởng nhóm có ID {oldLeaderId}...");
        team1.removeMember(oldLeaderId);
        Console.WriteLine("Kết quả nhóm hiện tại:");
        team1.displayTeam();
    }

    static void RunStep13()
    {
        if (emp1 == null) { Console.WriteLine("Vui lòng chạy Bước 1 trước!"); return; }
        Console.WriteLine("--- BƯỚC 13: TẠO NHÓM THỨ 2 VÀ CHIA SẺ THÀNH VIÊN ---");
        Console.WriteLine("Đang tạo nhóm Beta và thêm Employee 1 vào...");
        ProjectTeam team2 = new ProjectTeam("P002", "Dự án Beta", emp1);
        team2.addMember(new Employee("E003", "Hoang Van E", 1200.0));
        team2.displayTeam();
        Console.WriteLine($"Nhân sự {emp1.Id} hiện thuộc cả Nhóm 1 và Nhóm 2 (Kết tập - Aggregation).");
    }

    static void RunStep14()
    {
        Console.WriteLine("--- BƯỚC 14: HỦY NHÓM 2 (KẾT THÚC KHỐI LỆNH) ---");
        Console.WriteLine("Ghi chú: Trong C#, khi thoát khỏi hàm RunStep13, biến team2 đã hết scope.");
        Console.WriteLine("Đang gọi Garbage Collector (Bộ gom rác) để ép hủy đối tượng team2...");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Hoàn tất dọn dẹp!");
    }

    static void RunStep15()
    {
        if (emp1 == null) { Console.WriteLine("Vui lòng chạy Bước 1 trước!"); return; }
        Console.WriteLine("--- BƯỚC 15: KIỂM TRA NHÂN SỰ SAU KHI NHÓM 2 BỊ HỦY ---");
        Console.WriteLine("Mặc dù nhóm 2 đã bị hủy, Employee 1 vẫn tồn tại độc lập:");
        emp1.displayInfo();
    }
}
