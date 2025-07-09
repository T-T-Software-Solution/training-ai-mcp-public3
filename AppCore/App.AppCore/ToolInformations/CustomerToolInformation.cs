namespace App.AppCore.ToolInformations;

public static class CustomerToolInformation
{
    public const string GetCustomerById_Description = "Get customer details by customer ID. / ดึงข้อมูลลูกค้าด้วยรหัสลูกค้า (Customer ID must be a Guid)";
    public const string GetCustomerById_Id_Description = "Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)";

    public const string GetCustomerByPhone_Description = "Get customer details by phone number. / ดึงข้อมูลลูกค้าด้วยเบอร์โทรศัพท์";
    public const string GetCustomerByPhone_Phone_Description = "Customer phone number / เบอร์โทรศัพท์ลูกค้า";

    public const string GetAllCustomers_Description = "Get all customers. / ดึงข้อมูลลูกค้าทั้งหมด";

    public const string AddCustomer_Description = "Add a new customer. / เพิ่มลูกค้าใหม่";
    public const string AddCustomer_Name_Description = "Customer name / ชื่อลูกค้า";
    public const string AddCustomer_Phone_Description = "Customer phone number / เบอร์โทรศัพท์ลูกค้า";
    public const string AddCustomer_Email_Description = "Customer email / อีเมลลูกค้า";

    public const string UpdateCustomer_Description = "Update an existing customer. / แก้ไขข้อมูลลูกค้า";
    public const string UpdateCustomer_Id_Description = "Customer ID (Guid) / รหัสลูกค้า (ต้องเป็น Guid)";
    public const string UpdateCustomer_Name_Description = "Customer name / ชื่อลูกค้า";
    public const string UpdateCustomer_Phone_Description = "Customer phone number / เบอร์โทรศัพท์ลูกค้า";
    public const string UpdateCustomer_Email_Description = "Customer email / อีเมลลูกค้า";
}
