CREATE PROCEDURE sp_FormAdd
    @FormDate  date,  
    @FormCampus varchar(max),  
    @FormStatus varchar(max),
    @EmployeeId varchar(25),
    @EmployeeFirstName varchar(25),
    @EmployeeLastName varchar(25),
    @DepartmentId int
     
    AS  
    BEGIN  
        INSERT INTO form(form_date,form_campus,form_status,employee_id,employee_first_name,employee_last_name,department_id)  
        VALUES (@FormDate,@FormCampus,@FormStatus,@EmployeeId,@EmployeeFirstName,@EmployeeLastName,@DepartmentId)
    END
