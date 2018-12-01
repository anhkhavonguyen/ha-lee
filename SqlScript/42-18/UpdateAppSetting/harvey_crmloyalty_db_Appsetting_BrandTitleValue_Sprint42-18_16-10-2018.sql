INSERT INTO public."AppSettingTypes"("Id", "TypeName") 
	VALUES (6, 'Content');
	
INSERT INTO public."AppSettings"(
    "Id", "CreatedDate", "UpdatedDate", "CreatedBy", "UpdatedBy", "Name", "Value", "GroupName", "UserId", "AppSettingTypeId", "CreatedByName", "UpdateByName")
    VALUES (58, now(), now(), null, null, 'AcronymBrandTitleValue', 'TOG', 
							'AppTitleSetting', null, 6, null, null);
							
INSERT INTO public."AppSettings"(
    "Id", "CreatedDate", "UpdatedDate", "CreatedBy", "UpdatedBy", "Name", "Value", "GroupName", "UserId", "AppSettingTypeId", "CreatedByName", "UpdateByName")
    VALUES (59, now(), now(), null, null, 'BrandTitleValue', 'Toy Or Game', 
							'AppTitleSetting', null, 6, null, null);