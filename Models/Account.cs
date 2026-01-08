using Swagger.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#region Tables
public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [Key]
    public string UserId { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    public bool? IsAdmin { get; set; }       
    public bool? Status { get; set; }
    public DateTime AccountCreated { get; set; }
    public ICollection<UserPermission> UserPermissions { get; set; }
    public ICollection<Images> Images { get; set; }

}
public class UserPermission
{
    [Key]
    public int PermissionId { get; set; }
    [Required]  
    public string UserId { get; set; } //foreign key
    public int ModuleId { get; set; }
    public Account Account { get; set; }
    public Module Module { get; set; }
}
public class Module
{
    [Key] 
    public int ModuleId { get; set; }
    [Required]
    [MaxLength(100)] 
    public string ModuleName { get; set; }
    [MaxLength(255)] 
    public string Description { get; set; }
    public ICollection<UserPermission> UserPermissions { get; set; }
}

#endregion

#region Other - Classes

public class CreateUserAccount
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}
public class UpdatePassword
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

#endregion