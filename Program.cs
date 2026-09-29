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
        Console.WriteLine($"[{this.GetType().Name}] Mã NS: {Id}, Họ tên: {FullName}, Lương CB: {BaseSalary}");
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
        Console.WriteLine($"[{this.GetType().Name}] Mã NS: {Id}, Họ tên: {FullName}, Lương CB: {BaseSalary}, Ngôn ngữ: {PrimaryLanguage}, Phụ cấp: {TechnicalAllowance}");
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
        
        Employee? existing = _members.FirstOrDefault(m => m.Id == employee.Id);

        if (existing != null)
        {
            // Trùng ID nhưng khác thực thể (instance) -> Tức là cố tình add 1 người mới nhưng lấy mã của người cũ
            if (!ReferenceEquals(existing, employee))
            {
                Console.WriteLine($"[LỖI] Mã nhân sự {employee.Id} đang được sử dụng bởi người khác trong nhóm! Từ chối thêm.");
                return false; 
            }
            
            // Trùng ID và cùng 1 thực thể (chỉ muốn thăng chức cho người đang ở trong nhóm)
            if (!makeLeader)
            {
                Console.WriteLine($"Nhân sự {employee.Id} đã có trong nhóm.");
                return false;
            }
        }
        else
        {
            _members.Add(employee);
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
    // Kho lưu trữ TẤT CẢ nhân sự
    static List<Employee> allEmployees = new List<Employee>();
    
    // Kho lưu trữ TẤT CẢ nhóm dự án
    static List<ProjectTeam> allTeams = new List<ProjectTeam>();
    
    static Employee? sharedEmpForStep15; // Dùng để xác nhận nhân viên sống sót ở bước 15

    public static void Main()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\n================= MENU KIỂM THỬ ==================");
            Console.WriteLine("1.  Tạo 2 Employee bằng 2 constructor khác nhau");
            Console.WriteLine("2.  Tạo 2 SoftwareEngineer bằng 2 constructor khác nhau");
            Console.WriteLine("3.  Tăng lương nhân sự bằng số tiền cố định");
            Console.WriteLine("4.  Tăng lương nhân sự theo %");
            Console.WriteLine("5.  Tạo nhóm dự án mới");
            Console.WriteLine("6.  Thêm nhân sự vào nhóm");
            Console.WriteLine("7.  Tạo thêm 1 kỹ sư và thêm làm Trưởng nhóm");
            Console.WriteLine("8.  Thử thêm lại thành viên đã tồn tại vào nhóm");
            Console.WriteLine("9.  Hiển thị danh sách nhóm");
            Console.WriteLine("10. Tính tổng chi phí nhân sự hằng tháng của 1 nhóm");
            Console.WriteLine("11. Thử xóa trưởng nhóm hiện tại");
            Console.WriteLine("12. Đổi trưởng nhóm rồi xóa cựu trưởng nhóm");
            Console.WriteLine("13. Tạo nhóm mới và chia sẻ thành viên");
            Console.WriteLine("14. Hủy một nhóm dự án");
            Console.WriteLine("15. Kiểm tra nhân sự sau khi nhóm bị hủy");
            Console.WriteLine("0.  Thoát chương trình");
            Console.WriteLine("==================================================");
            Console.Write("Mời bạn chọn chức năng (0-15): ");
            
            string? choice = Console.ReadLine();
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
        Console.Write($"{prompt}{(string.IsNullOrEmpty(defaultValue) ? "" : $" [{defaultValue}]")}: ");
        string? input = Console.ReadLine();

        // Hỗ trợ người dùng nhập "" hoặc "   " để cố tình test trường hợp biên (chuỗi rỗng)
        if (input != null && input.StartsWith("\"") && input.EndsWith("\"") && input.Length >= 2)
        {
            return input.Substring(1, input.Length - 2); 
        }

        return string.IsNullOrWhiteSpace(input) ? defaultValue : input!;
    }

    static Employee? SelectEmployee(string promptMsg = "Chọn nhân sự")
    {
        if (allEmployees.Count == 0)
        {
            Console.WriteLine("Chưa có nhân sự nào được tạo. Vui lòng chạy Bước 1 hoặc Bước 2 trước để tạo mới!");
            return null;
        }

        Console.WriteLine("\nDanh sách TẤT CẢ nhân sự hiện có:");
        for (int i = 0; i < allEmployees.Count; i++)
        {
            Console.WriteLine($"{i + 1}. [{allEmployees[i].GetType().Name}] {allEmployees[i].Id} - {allEmployees[i].FullName} (Lương CB: {allEmployees[i].BaseSalary})");
        }

        while (true)
        {
            string? choiceStr = GetInput($"\n{promptMsg} (1-{allEmployees.Count})", "1");
            if (int.TryParse(choiceStr, out int choice) && choice >= 1 && choice <= allEmployees.Count)
            {
                return allEmployees[choice - 1];
            }
            Console.WriteLine("Lựa chọn không hợp lệ, vui lòng thử lại.");
        }
    }

    static ProjectTeam? SelectTeam(string promptMsg = "Chọn nhóm dự án")
    {
        if (allTeams.Count == 0)
        {
            Console.WriteLine("Chưa có nhóm dự án nào. Vui lòng chạy Bước 5 trước để tạo nhóm!");
            return null;
        }

        Console.WriteLine("\nDanh sách TẤT CẢ nhóm dự án hiện có:");
        for (int i = 0; i < allTeams.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {allTeams[i].ProjectName} ({allTeams[i].ProjectCode}) - Thành viên: {allTeams[i].calculateTotalMonthlyCost()} VNĐ/Tháng");
        }

        while (true)
        {
            string? choiceStr = GetInput($"\n{promptMsg} (1-{allTeams.Count})", "1");
            if (int.TryParse(choiceStr, out int choice) && choice >= 1 && choice <= allTeams.Count)
            {
                return allTeams[choice - 1];
            }
            Console.WriteLine("Lựa chọn không hợp lệ, vui lòng thử lại.");
        }
    }

    static void RunStep1()
    {
        Console.WriteLine("--- BƯỚC 1: TẠO 2 EMPLOYEE BẰNG 2 CONSTRUCTOR KHÁC NHAU ---");
        
        Console.WriteLine("\n* Tạo Employee thứ nhất (Dùng constructor 2 tham số - Lương CB mặc định = 0):");
        string id1 = GetInput("Nhập Mã NS 1", $"E{allEmployees.Count + 1:000}");
        string name1 = GetInput("Nhập Họ tên 1", "Nguyen Van A");
        Employee e1 = new Employee(id1, name1);
        allEmployees.Add(e1);
        e1.displayInfo();

        Console.WriteLine("\n* Tạo Employee thứ hai (Dùng constructor 3 tham số - Có truyền Lương CB):");
        string id2 = GetInput("Nhập Mã NS 2", $"E{allEmployees.Count + 1:000}");
        string name2 = GetInput("Nhập Họ tên 2", "Tran Thi B");
        double salary2 = double.Parse(GetInput("Nhập Lương cơ bản 2", "1500"));
        Employee e2 = new Employee(id2, name2, salary2);
        allEmployees.Add(e2);
        e2.displayInfo();
        
        Console.WriteLine("\n=> Đã tạo xong 2 Employee và đưa vào kho lưu trữ!");
    }

    static void RunStep2()
    {
        Console.WriteLine("--- BƯỚC 2: TẠO 2 SOFTWARE ENGINEER BẰNG 2 CONSTRUCTOR KHÁC NHAU ---");
        
        Console.WriteLine("\n* Tạo Kỹ sư thứ nhất (Dùng constructor 3 tham số):");
        string id1 = GetInput("Nhập Mã KS 1", $"SE{allEmployees.Count + 1:000}");
        string name1 = GetInput("Nhập Họ tên 1", "Le Van C");
        string lang1 = GetInput("Nhập Ngôn ngữ chính 1", "C#");
        SoftwareEngineer se1 = new SoftwareEngineer(id1, name1, lang1);
        allEmployees.Add(se1);
        se1.displayInfo();

        Console.WriteLine("\n* Tạo Kỹ sư thứ hai (Dùng constructor 5 tham số):");
        string id2 = GetInput("Nhập Mã KS 2", $"SE{allEmployees.Count + 1:000}");
        string name2 = GetInput("Nhập Họ tên 2", "Pham Thi D");
        double salary2 = double.Parse(GetInput("Nhập Lương cơ bản 2", "2000"));
        string lang2 = GetInput("Nhập Ngôn ngữ chính 2", "Java");
        double allow2 = double.Parse(GetInput("Nhập Phụ cấp 2", "500"));
        SoftwareEngineer se2 = new SoftwareEngineer(id2, name2, salary2, lang2, allow2);
        allEmployees.Add(se2);
        se2.displayInfo();
        
        Console.WriteLine("\n=> Đã tạo xong 2 Kỹ sư phần mềm và đưa vào kho lưu trữ!");
    }

    static void RunStep3()
    {
        Console.WriteLine("--- BƯỚC 3: TĂNG LƯƠNG CỐ ĐỊNH ---");
        Employee? target = SelectEmployee("Chọn nhân sự muốn tăng lương");
        if (target == null) return;

        Console.WriteLine("\nThông tin nhân sự đã chọn:");
        target.displayInfo();
        double amount = double.Parse(GetInput("Nhập số tiền muốn tăng cho nhân sự này", "200"));
        target.increaseSalary(amount);
        Console.WriteLine("\nSau khi tăng:");
        target.displayInfo();
    }

    static void RunStep4()
    {
        Console.WriteLine("--- BƯỚC 4: TĂNG LƯƠNG THEO % ---");
        Employee? target = SelectEmployee("Chọn nhân sự muốn tăng lương");
        if (target == null) return;

        Console.WriteLine("\nThông tin nhân sự đã chọn:");
        target.displayInfo();
        double percent = double.Parse(GetInput("Nhập % muốn tăng", "10"));
        target.increaseSalary(percent, true);
        Console.WriteLine("\nSau khi tăng:");
        target.displayInfo();
    }

    static void RunStep5()
    {
        Console.WriteLine("--- BƯỚC 5: TẠO NHÓM DỰ ÁN MỚI ---");
        string code = GetInput("Nhập mã dự án", "");
        string name = GetInput("Nhập tên dự án", "");
        ProjectTeam team = new ProjectTeam(code, name);
        allTeams.Add(team);
        Console.WriteLine("Đã tạo nhóm thành công và thêm vào danh sách quản lý!");
        team.displayTeam();
    }

    static void RunStep6()
    {
        Console.WriteLine("--- BƯỚC 6: THÊM NHÂN SỰ VÀO NHÓM ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm muốn nhận thêm thành viên");
        if (team == null) return;

        Employee? target = SelectEmployee("Chọn nhân sự muốn thêm vào nhóm");
        if (target == null) return;

        Console.WriteLine($"Đang thêm {target.FullName} vào nhóm {team.ProjectName}...");
        if (team.addMember(target))
            Console.WriteLine("Thêm thành công!");
    }

    static void RunStep7()
    {
        Console.WriteLine("--- BƯỚC 7: TẠO THÊM 1 KỸ SƯ VÀ ĐẶT LÀM TRƯỞNG NHÓM ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm muốn nạp trưởng nhóm");
        if (team == null) return;

        Console.WriteLine("\n[TÌNH TRẠNG NHÓM TRƯỚC KHI THÊM LÃNH ĐẠO MỚI]");
        team.displayTeam();

        Console.WriteLine("\n* Khởi tạo Kỹ sư MỚI (Dùng constructor 5 tham số):");
        string id = GetInput("Nhập Mã KS", $"SE{allEmployees.Count + 1:000}");
        string name = GetInput("Nhập Họ tên", "Trưởng Nhóm Siêu Việt");
        double salary = double.Parse(GetInput("Nhập Lương cơ bản", "3000"));
        string lang = GetInput("Nhập Ngôn ngữ chính", "C++");
        double allow = double.Parse(GetInput("Nhập Phụ cấp", "1000"));
        
        SoftwareEngineer newSE = new SoftwareEngineer(id, name, salary, lang, allow);
        allEmployees.Add(newSE);
        
        Console.WriteLine($"\nĐang thêm {newSE.FullName} vào nhóm {team.ProjectName} làm trưởng nhóm...");
        if (team.addMember(newSE, true))
        {
            Console.WriteLine("Tạo Kỹ sư mới và cấp quyền trưởng nhóm thành công!");
            Console.WriteLine("\n[TÌNH TRẠNG NHÓM SAU KHI THÊM LÃNH ĐẠO MỚI]");
            team.displayTeam();
        }
    }

    static void RunStep8()
    {
        Console.WriteLine("--- BƯỚC 8: THỬ THÊM LẠI THÀNH VIÊN ĐÃ TỒN TẠI ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm dự án để test");
        if (team == null) return;

        // Cho phép chọn trực tiếp người từ trong kho để giữ nguyên lương/thông tin
        Employee? target = SelectEmployee("Chọn nhân sự (hãy chọn người ĐÃ CÓ trong nhóm trên)");
        if (target == null) return;
        
        Console.WriteLine($"\nĐang thử thêm {target.FullName} (Mã: {target.Id}) vào nhóm làm nhân viên thường...");
        
        // Gọi hàm addMember 1 tham số (thêm làm nhân viên thường)
        if (team.addMember(target))
        {
            Console.WriteLine("Thêm thành công!");
        }
        else
        {
            Console.WriteLine("Thêm thất bại (đã bị chặn do trùng lặp)!");
        }
    }

    static void RunStep9()
    {
        Console.WriteLine("--- BƯỚC 9: HIỂN THỊ DANH SÁCH NHÓM ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm muốn hiển thị");
        if (team == null) return;

        team.displayTeam();
    }

    static void RunStep10()
    {
        Console.WriteLine("--- BƯỚC 10: TÍNH TỔNG CHI PHÍ NHÂN SỰ ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm muốn tính tiền");
        if (team == null) return;

        Console.WriteLine($"Tổng chi phí hằng tháng của nhóm {team.ProjectName}: {team.calculateTotalMonthlyCost()}");
    }

    static void RunStep11()
    {
        Console.WriteLine("--- BƯỚC 11: THỬ XÓA TRƯỞNG NHÓM HIỆN TẠI ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm dự án");
        if (team == null) return;

        if (team.Leader == null)
        {
            Console.WriteLine("Nhóm này chưa có trưởng nhóm nên không thể test tính năng này. Hãy chạy Bước 7 trước!");
            return;
        }

        Console.WriteLine($"Đang thử xóa trưởng nhóm {team.Leader.FullName}...");
        team.removeMember(team.Leader.Id);
    }

    static void RunStep12()
    {
        Console.WriteLine("--- BƯỚC 12: ĐỔI TRƯỞNG NHÓM RỒI XÓA CỰU TRƯỞNG NHÓM ---");
        ProjectTeam? team = SelectTeam("Chọn nhóm dự án");
        if (team == null) return;

        if (team.Leader == null)
        {
            Console.WriteLine("Nhóm này chưa có trưởng nhóm cũ để đổi. Hãy chạy Bước 7 trước!");
            return;
        }
        
        Employee? newLeader = SelectEmployee("Chọn nhân sự thay thế làm trưởng nhóm MỚI");
        if (newLeader == null) return;

        string oldLeaderId = team.Leader.Id;
        Console.WriteLine($"\n1. Đổi trưởng nhóm sang {newLeader.FullName}...");
        team.changeLeader(newLeader);
        Console.WriteLine($"2. Xóa cựu trưởng nhóm có ID {oldLeaderId}...");
        team.removeMember(oldLeaderId);
        Console.WriteLine("\nKết quả nhóm hiện tại:");
        team.displayTeam();
    }

    static void RunStep13()
    {
        Console.WriteLine("--- BƯỚC 13: TẠO NHÓM MỚI VÀ CHIA SẺ THÀNH VIÊN ---");
        Employee? target = SelectEmployee("Chọn nhân sự muốn chia sẻ cho dự án mới này");
        if (target == null) return;
        
        sharedEmpForStep15 = target;

        Console.WriteLine("\nĐang tạo nhóm mới và đưa nhân sự này vào...");
        string code = GetInput("Nhập mã dự án", $"P{allTeams.Count + 1:000}");
        string name = GetInput("Nhập tên dự án", "Dự án Beta (Chia sẻ)");
        
        ProjectTeam team2 = new ProjectTeam(code, name, target);
        allTeams.Add(team2);
        
        team2.displayTeam();
        Console.WriteLine($"\nNhân sự {target.Id} hiện đã được dùng chung cho Nhóm mới này (Kết tập - Aggregation).");
    }

    static void RunStep14()
    {
        Console.WriteLine("--- BƯỚC 14: XÓA HỦY MỘT NHÓM DỰ ÁN ---");
        ProjectTeam? targetTeam = SelectTeam("Chọn nhóm dự án muốn hủy");
        if (targetTeam == null) return;
        
        allTeams.Remove(targetTeam);
        Console.WriteLine($"Đã xóa dự án {targetTeam.ProjectName} khỏi danh sách quản lý.");
        
        Console.WriteLine("Đang gọi Garbage Collector (Bộ gom rác) để ép hủy đối tượng Team trên RAM...");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Hoàn tất dọn dẹp!");
    }

    static void RunStep15()
    {
        if (sharedEmpForStep15 == null) 
        { 
            Console.WriteLine("Vui lòng chạy Bước 13 trước để đánh dấu nhân sự được chia sẻ!"); 
            return; 
        }
        Console.WriteLine("--- BƯỚC 15: KIỂM TRA NHÂN SỰ SAU KHI NHÓM BỊ HỦY ---");
        Console.WriteLine("Mặc dù nhóm chứa nhân sự này có thể đã bị xóa ở Bước 14, bản thân nhân sự vẫn tồn tại độc lập:");
        sharedEmpForStep15.displayInfo();
    }
}
