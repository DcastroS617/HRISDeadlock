

-- ====================================================================================
-- SCRIPT DE LIMPIEZA DE DATOS DE CAPACITACIÓN ENVUELTO EN UNA TRANSACCIÓN SEGURA
-- Garantiza que todas las operaciones se completen o ninguna lo haga.
-- ====================================================================================

BEGIN TRANSACTION; -- Inicia la transacción

BEGIN TRY
    PRINT 'Iniciando limpieza de datos de capacitación...';

    -- PASO 1: Eliminar registros de las tablas de enlace (tablas "hijo") primero.
    -- Esto previene la mayoría de los errores de llaves foráneas (FK).
    PRINT 'Limpiando tablas de enlace...';

    DELETE FROM [Training].[TrainersByCourses];
    DELETE FROM [Training].[CoursesByTrainingPrograms];
    DELETE FROM [Training].[CoursesByThematicAreas];
    DELETE FROM [Training].[SchoolTrainingByCouses];
    
    -- PASO 2: Eliminar registros de las tablas principales (tablas "padre").
    PRINT 'Limpiando tablas principales...';
    
    DELETE FROM [Training].Classrooms
WHERE ClassroomCode IN ('099S1', '099S2','099S3',  'scripttest','AB001','AU001', 'AU002', 'AD001', 'AD002', 'AE001', 'AE002','AE003', 'ADU001', 'AM001', 'AVU001', 'ADU001', 'AM001', 'Escripttes', 'AUR001', 'AUS001')

DELETE FROM [Training].TrainingCenters 
WHERE TrainingCenterCode IN ('TC001', 'TC002', 'TC003', 'CC001', 'CC002','scripttest', 'CEU001', 'CEU002', 'CVU001', 'CEC001', 'CCE001', 'CCEL001', 'CCEL002', 'CAC001', 'CSE001', 'Escripttes',  'CSE002', 'CF001', 'Centroscri') 

DELETE FROM [Training].[TrainingPrograms] WHERE [TrainingProgramCode] IN ( 'P001','P002', 'PDU001', 'PDU002', 'PROscriptt', 'POU001', 'POU002', 'POC001', 'PE01', 'PE02', 'PUA001', 'PUS001', 'POE001', 'PF001', 'PROPscript', 'PROUNO001', 'PROUNO002')

DELETE FROM [Training].[TypeTraining]  WHERE typeTrainingCode IN ( 'TP001','TP002', 'TFD001', 'TFD002','TFCscriptt', 'POE001', 'TFCH001', 'TFEM001', 'TFF001', 'TFCHAscrip', 'TFE001', 'TFEL01', 'TFEL02','TFAC001', 'TFSOBRE_00')

DELETE FROM [Training].[SchoolTraining]  WHERE schoolTrainingCode IN ( 'SH001','SH002', 'SHD001', 'SHD002','SHOscriptt', 'SHU001', 'SHC001', 'SHEM001', 'SHEL001', 'SHEL002', 'SHAC001', 'SHSOB_001', 'SHF001', 'SCHOOLscri', 'ESCuscript', 'SHOCUR001', 'SHOUNO001', 'SHOUNO002')

DELETE FROM [Training].[ThematicAreas]  WHERE ThematicAreaCode IN ( 'ATH001', 'ATH002','ATD001','ATD002', 'ATHscriptt', 'ATE001','ATC001', 'ATED001', 'ATEL001', 'ATEL002', 'ATAC001', 'ATSOB_001', 'ATF001', 'AREATEscri', 'ARECUR001', 'AREUNO002', 'AREUNO001', 'AREUNO002')

DELETE FROM [Training].[Courses]  WHERE CourseCode IN ( 'CU001', 'CU002','CUD001','CUD002', 'COUscriptt', 'CUUP001','COC001', 'COEM001','COEL001', 'COEL002', 'SHAC001', 'COSOB_001')

DELETE FROM [Training].[Trainers]  WHERE TrainerCode IN ( 'PE001','PEA001', 'PEA002')
   
DELETE FROM [Training].[TrainersByCourses]  WHERE TrainerCode IN ('PE001')

DELETE FROM[Training].[TrainingPrograms]  WHERE TrainingProgramCode IN ('PRO001', 'PROUNO001', 'PROUNO002')
	   
DELETE FROM[Training].[CoursesByTrainingPrograms]  WHERE TrainingProgramCode IN ('PRO001')

DELETE FROM[Training].[CoursesByThematicAreas]  WHERE ThematicAreaCode IN ('ARECUR001', 'AREUNO002','AREUNO001', 'AREUNO002')

DELETE FROM[Training].[SchoolTrainingByCouses]  WHERE SchoolTrainingCode IN ('SHOCUR001', 'SHOUNO001', 'SHOUNO002', 'SHOUNO001','SHOUNO002' )

DELETE FROM [Training].[TrainersByCourses]  WHERE [TrainerCode] IN ('PEA001', 'PEA002')

DELETE FROM [Training].[CoursesByTrainingPrograms]  WHERE [TrainingProgramCode] IN ('PROUNO001', 'PROUNO002')

DELETE FROM [HRIS_Testing].Training.ClassificationCourse where ClassificationCourseCode IN ('CCL001','CCL002', 'CCLA001', 'CCLA002','CLAscriptt', 'CCLA01', 'CCCE001', 'CUCL001', 'CCELIM001', 'CCELIM002', 'CCRA001', 'CCSOB_001', 'CCFL001', 'CURCLAscri') 

 -- PASO 1: Eliminar dependencias en [MatrixTargetByCostFarms]
    PRINT 'Paso 1: Eliminando registros de MatrixTargetByCostFarms...';
    DELETE FROM [Training].[MatrixTargetByCostFarms]
    WHERE MatrixTargetId IN (
        -- Subconsulta para obtener los IDs numéricos de los códigos 'MA001', 'MA002'
        SELECT MatrixTargetId FROM [Training].[MatrixTarget] WHERE MatrixTargetCode IN ('MA001', 'MA002', 'MAD001', 'MAD002')
    );

    -- PASO 2: Eliminar dependencias en [MatrixTargetByCostMiniZones]
    PRINT 'Paso 2: Eliminando registros de MatrixTargetByCostMiniZones...';
    DELETE FROM [Training].[MatrixTargetByCostMiniZones]
    WHERE MatrixTargetId IN (
        SELECT MatrixTargetId FROM [Training].[MatrixTarget] WHERE MatrixTargetCode IN ('MA001', 'MA002', 'MAD001', 'MAD002')
    );

    -- PASO 3: Eliminar dependencias en [MatrixTargetByCostZones]
    PRINT 'Paso 3: Eliminando registros de MatrixTargetByCostZones...';
    DELETE FROM [Training].[MatrixTargetByCostZones]
    WHERE MatrixTargetId IN (
        SELECT MatrixTargetId FROM [Training].[MatrixTarget] WHERE MatrixTargetCode IN ('MA001', 'MA002', 'MAD001', 'MAD002')
    );
    
    -- PASO 4: (Opcional) Si existen otras tablas ligadas, añádelas aquí siguiendo el mismo patrón.
    -- Por ejemplo, la que causó el error anterior:
    PRINT 'Paso 4: Eliminando registros de MatrixTargetByDivisions (del ejemplo anterior)...';
    DELETE FROM [Training].[MatrixTargetByDivisions]
    WHERE MatrixTargetId IN (
        SELECT MatrixTargetId FROM [Training].[MatrixTarget] WHERE MatrixTargetCode IN ('MA001', 'MA002', 'MAD001', 'MAD002')
    );

    -- PASO FINAL: Eliminar los registros 'padre' en [MatrixTarget]
    PRINT 'Paso Final: Eliminando registros principales de MatrixTarget...';
    DELETE FROM [Training].[MatrixTarget]
    WHERE [MatrixTargetCode] IN ('MA001', 'MA002', 'MAD001', 'MAD002'); -- Limpiamos los códigos que especificaste.

    -- Si no hubo errores, confirma todos los cambios.
    COMMIT TRANSACTION;
    PRINT 'Transacción completada exitosamente. Todos los datos han sido eliminados y guardados.';

END TRY
BEGIN CATCH
    -- Si cualquier sentencia en el bloque TRY falla, entra aquí.
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION; -- Revierte todos los cambios hechos desde el BEGIN TRANSACTION.

    PRINT '¡ERROR! Ocurrió un problema durante la limpieza. La transacción ha sido revertida.';
    
    -- Muestra el mensaje y la línea del error original para facilitar la depuración.
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorLine INT = ERROR_LINE();
    PRINT 'Mensaje de error: ' + @ErrorMessage;
    PRINT 'Línea del error: ' + CAST(@ErrorLine AS VARCHAR(10));
    
    -- Opcionalmente, vuelve a lanzar el error.
    -- THROW; 
END CATCH;
GO


