/*Organization  ID
Org Admin ID
User Status
Business User ID
Username  
User Password
User Mobile Number
User National ID
User Email
*/

//add using statements
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace AgentCheckListApi.Model{
// Create Class User with Properties
[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("organizationId")]
    public string? OrganizationId { get; set; }
    [Required]
    [BsonElement("orgAdminId")]

    public string OrgAdminId { get; set; }
    [Required]
    [BsonElement("userStatus")]

    public bool IsActive { get; set; }
    [Required]
    //businessUserId
    [BsonElement("businessUserId")]
    public int BusinessUserId { get; set; }
    [Required]
//username
    [BsonElement("username")]
    [BsonRepresentation(BsonType.String)]
    public string Username { get; set; }
    [Required]
//userPassword
    [BsonElement("userPassword")]
    [BsonRepresentation(BsonType.String)]
    public string UserPassword { get; set; }
    [Required]
    [BsonElement("userMobileNumber")]
    public string UserMobileNumber { get; set; }
    [Required]
    [BsonElement("userNationalId")]
    public string UserNationalId { get; set; }
    [Required]

    public string UserEmail { get; set; }
}
}