use CompetencyMatrix

delete from SkillLevelCriteria
delete from SkillCriteria
delete from PositionMatrixSkill
delete from Skill
delete from SkillCategory
delete from SkillEvaluationModelLevel
delete from SkillEvaluationModel
delete from SkillLevelModel


declare @rootCatId int
declare @skillIdCSharp int
declare @skillIdEnglish int
declare @skillIdImagination int

declare @jseSkillLevelModelId int
declare @mseSkillLevelModelId int
declare @sseSkillLevelModelId int

declare @beginnerSkillLevelModelId int
declare @intermediateSkillLevelModelId int
declare @upperIntermediateSkillLevelModelId int
declare @masterSkillLevelModelId int

declare @imaginationSkillLevelModelId_acolyte int
declare @imaginationSkillLevelModelId_enoite int

declare @skillEvaluationModelIdSoftware int
declare @skillEvaluationModelIdLanguage int
declare @skillEvaluationModelIdImagination int

----------------------------------------------------------------------------------------------------------------------------------------------
insert into SkillCategory (Name, ParentId)
values ('Root Category', null)
set @rootCatId = IDENT_CURRENT('SkillCategory')

----------------------------------------------------------------------------------------------------------------------------------------------
--Imagination
insert into SkillLevelModel (Name, Description, Quality)
values ('Acolyte', '', 1)
set @imaginationSkillLevelModelId_acolyte = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('Enoite', '', 2)
set @imaginationSkillLevelModelId_enoite = IDENT_CURRENT('SkillLevelModel')

--Dev
insert into SkillLevelModel (Name, Description, Quality)
values ('JSE', 'Junior software developer', 1)
set @jseSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('MSE', 'Middle software developer', 2)
set @mseSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('SSE', 'Senior software engineer', 3)
set @sseSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

--English
insert into SkillLevelModel (Name, Description, Quality)
values ('Beginner', 'Beginner level', 1)
set @beginnerSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('Intermediate', 'Intermediate', 2)
set @intermediateSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('Upper-intermediate', 'Upper intermediate', 3)
set @upperIntermediateSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

insert into SkillLevelModel (Name, Description, Quality)
values ('Master', 'Master', 4)
set @masterSkillLevelModelId = IDENT_CURRENT('SkillLevelModel')

--------------------------------------------------------------------------
insert into SkillEvaluationModel (Name, Description)
values ('Imaginarium model', 'This is a basic evaluation model for imaginary skills')
set @skillEvaluationModelIdImagination = IDENT_CURRENT('SkillEvaluationModel')

insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdSoftware, @jseSkillLevelModelId)

insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdSoftware, @mseSkillLevelModelId)


----------------------------------------------------------------------------------------------------------------------------------------------
insert into SkillEvaluationModel (Name, Description)
values ('Development model', 'This is a basic evaluation model for software developer skills')
set @skillEvaluationModelIdSoftware = IDENT_CURRENT('SkillEvaluationModel')

insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdSoftware, @jseSkillLevelModelId)
insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdSoftware, @mseSkillLevelModelId)
insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdSoftware, @sseSkillLevelModelId)

----------------------------------------------------------------------------------------------------------------------------------------------
insert into SkillEvaluationModel (Name, Description)
values ('Language model', 'This is a basic evaluation model for language skills')
set @skillEvaluationModelIdLanguage = IDENT_CURRENT('SkillEvaluationModel')

insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdLanguage, @beginnerSkillLevelModelId)
insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdLanguage, @intermediateSkillLevelModelId)
insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdLanguage, @upperIntermediateSkillLevelModelId)
insert into SkillEvaluationModelLevel (SkillEvaluationModelId, SkillLevelModelId)
values (@skillEvaluationModelIdLanguage, @masterSkillLevelModelId)

----------------------------------------------------------------------------------------------------------------------------------------------
insert into Skill (CategoryId, EvaluationModelId, Name, Description, TrainingMaterials )
values (@rootCatId, @skillEvaluationModelIdSoftware, 'C#', 'C# proficiency', '')
set @skillIdCSharp = IDENT_CURRENT('Skill')

insert into Skill (CategoryId, EvaluationModelId, Name, Description, TrainingMaterials )
values (@rootCatId, @skillEvaluationModelIdSoftware, 'English', 'English language proficiency', '')
set @skillIdEnglish = IDENT_CURRENT('Skill')

----------------------------------------------------------------------------------------------------------------------------------------------
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdCSharp, 'Collections', 'S')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdCSharp, 'Generics', 'P')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdCSharp, 'Multithreading', 'E')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdCSharp, 'Network', 'C')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdCSharp, 'Services', 'I')

----------------------------------------------------------------------------------------------------------------------------------------------
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdEnglish, 'Irregular verbs', 'S')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdEnglish, 'Proper english accent', 'P')
insert into SkillCriteria (SkillId, Name, Description)
values (@skillIdEnglish, 'More than 10000 words', 'E')

/*

POSITION MATRIX INIT

*/

if not exists (select * from SkillGroupType where Name = 'Is a Must')
insert into SkillGroupType (Id, Name) values (1, 'Is a Must')
if not exists (select * from SkillGroupType where Name = 'Is a Plus')
insert into SkillGroupType (Id, Name) values (2, 'Is a Plus')
if not exists (select * from SkillGroupType where Name = 'One in Group')
insert into SkillGroupType (Id, Name) values (3, 'One in Group')

---Clear
delete from PositionMatrixInheritance;
delete from PositionMatrixSkill;
delete from PositionMatrixSkillGroup;
delete from PositionMatrix;

---Sample
insert into PositionMatrix (Name, Description)
values ('Project Manager', 'Project Manager position requirements')
insert into PositionMatrix (Name, Description)
values ('HTML Developer', 'HTML Developer position requirements')

insert into PositionMatrix (Name, Description)
values ('Jr. C# Developer', 'Jr. C# dev position requirements')
declare @jrCSharpMatrix int
set @jrCSharpMatrix = IDENT_CURRENT('PositionMatrix')

insert into PositionMatrix (Name, Description)
values ('Sr. C# Developer', 'Sr. C# dev position requirements')
declare @srCSharpMatrix int
set @srCSharpMatrix = IDENT_CURRENT('PositionMatrix')

insert into PositionMatrixInheritance (MatrixId, ParentMatrixId) values (@srCSharpMatrix, @jrCSharpMatrix)

declare @skillGroupIdPlus int
declare @skillGroupIdMust int

insert into PositionMatrixSkillGroup (Name, GroupTypeId) values ('Plus skill group', 2)
set @skillGroupIdPlus = IDENT_CURRENT('PositionMatrixSkillGroup')

insert into PositionMatrixSkillGroup (Name, GroupTypeId) values ('Must skill group', 1)
set @skillGroupIdMust = IDENT_CURRENT('PositionMatrixSkillGroup')

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@jrCSharpMatrix, @skillGroupIdMust, @skillIdCSharp, @jseSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@jrCSharpMatrix, @skillGroupIdMust, @skillIdEnglish, @beginnerSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@jrCSharpMatrix, @skillGroupIdPlus, @skillIdCSharp, @mseSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@jrCSharpMatrix, @skillGroupIdPlus, @skillIdEnglish, @intermediateSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@srCSharpMatrix, @skillGroupIdMust, @skillIdCSharp, @mseSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@srCSharpMatrix, @skillGroupIdMust, @skillIdEnglish, @intermediateSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@srCSharpMatrix, @skillGroupIdPlus, @skillIdCSharp, @sseSkillLevelModelId)

insert into PositionMatrixSkill (MatrixId, SkillGroupId, SkillId, SkillLevelId) 
values (@srCSharpMatrix, @skillGroupIdPlus, @skillIdEnglish, @masterSkillLevelModelId)
