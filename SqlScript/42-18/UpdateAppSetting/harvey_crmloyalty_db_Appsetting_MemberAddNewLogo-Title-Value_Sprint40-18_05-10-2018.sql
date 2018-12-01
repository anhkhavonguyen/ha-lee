UPDATE "AppSettings" 
	SET ("CreatedDate", "UpdatedDate", "CreatedBy", "UpdatedBy", "Name", "Value", "GroupName", "UserId", "AppSettingTypeId", "CreatedByName", "UpdateByName")
	= (now(), now(), null, null, 'MemberHomeContentContactInfo', '{"title" : "Contact Info","openTime" :"Monday to Sunday:<br/>11:00AM - 9:30PM","email" :"ecommerce@toyorgame.com.sg","phone" :"+65 6634 0240"}', 'MemberContactInfo', null, '3', null, null)
	WHERE "Id" = '53';