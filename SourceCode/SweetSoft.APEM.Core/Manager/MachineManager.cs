﻿using SweetSoft.APEM.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Manager
{
    public class MachineManager
    {
        public static void Delete(int id)
        {
            new TblMachineController().Delete(id);
        }

        public static TblMachine Insert(TblMachine item)
        {
            return new TblMachineController().Insert(item);
        }

        public static TblMachineCollection SelectAll()
        {
            return new TblMachineController().FetchAll();
        }

        public static TblMachineCollection SelectAllByDeparment(int DepartmentID)
        {
            if (DepartmentID > 0)
                return new SubSonic.Select().From(TblMachine.Schema)
                    .Where(TblMachine.DepartmentIDColumn).IsEqualTo(DepartmentID)
                    .ExecuteAsCollection<TblMachineCollection>();
            else
                return null;
        }

        public static TblMachine SelectMachineByID(int machineID)
        {
            return new TblMachineController().FetchByID(machineID).FirstOrDefault();
        }

        public static TblMachine Update(TblMachine item)
        {
            return new TblMachineController().Update(item);
        }
    }
}
