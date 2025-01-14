﻿using BloodAlcoholCalculator.Data;
using BloodAlcoholCalculator.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BloodAlcoholCalculator.Repository
{

     public delegate void RepositoryChangedEventHandler(object sender, EventArgs e);

     /// <summary>
     /// DataRepository wrapped around one specific db table
     /// </summary>
     /// <typeparam name="VmType"></typeparam>
     public class DataRepository<VmType, ModelType> where VmType : ViewModelBase, ViewModelConvertor<VmType, ModelType>, new()
     {
          private static DataRepository<VmType, ModelType> _baseRepositoryInstance;
          private ObservableCollection<VmType> _collection = new ObservableCollection<VmType>();
          private string dbPath { get; set; }

          public Dictionary<long, VmType> Dict { get; set; } = new Dictionary<long, VmType>();
          public ObservableCollection<VmType> Collection => _collection;

          public static DataRepository<VmType, ModelType> Instance
          {
               get {
                    if (_baseRepositoryInstance == null) {
                         _baseRepositoryInstance = new DataRepository<VmType, ModelType>();
                    }
                    return _baseRepositoryInstance;
               }
          }

          public DataRepository()
          {
               InitializeFromDatabase(PathSettings.Default.DatabasePath);
          }

          public void InitializeFromDatabase(string filePath)
          {
               dbPath = filePath;
               using (MainDatabase db = new MainDatabase(filePath)) {
                    var table = db.Context.GetTable(typeof(ModelType));

                    var list = new List<VmType>();
                    
                    foreach (var v in table) {
                         VmType t = new VmType();
                         t.ConvertFrom((ModelType)v);
                         list.Add(t);
                         Dict.Add(t.GetBaseId(), t);
                    }
                    list.Sort();
                    _collection = new ObservableCollection<VmType>(list);
               };
          }

          public event RepositoryChangedEventHandler Changed;

          protected virtual void OnChanged(EventArgs e)
          {
               if(Changed != null) {
                    Changed(this, e);
               }
          }

          public void Add(VmType t)
          {
               using (MainDatabase db = new MainDatabase(dbPath)) {
                    db.Add(t.GetBaseType());
               };

               Collection.Add(t);
               Dict.Add(t.GetBaseId(), t);
               OnChanged(new EventArgs() { });
          }

          public void Edit(VmType t)
          {
               using (MainDatabase db = new MainDatabase(dbPath)) {
                    db.Attach(t.GetBaseType());
                    db.SubmitChanges();
               };

               Dict.Remove(t.GetBaseId());
               Dict.Add(t.GetBaseId(), t);
               OnChanged(new EventArgs() { });
          }

          public void Remove(VmType t)
          {
               using (MainDatabase db = new MainDatabase(dbPath)) {
                    db.Attach(t.GetBaseType());
                    db.Remove(t.GetBaseType());
                    db.SubmitChanges();
               };
               Collection.Remove(t);
               Dict.Remove(t.GetBaseId());
               OnChanged(new EventArgs() { });
          }

          public void ClearTable()
          {
               using (MainDatabase db = new MainDatabase(dbPath)) {
                    foreach(var v in Dict.Values) {
                         db.Attach(v.GetBaseType());
                         db.Remove(v.GetBaseType());
                    }
                    db.SubmitChanges();
               };
               Collection.Clear();
               Dict.Clear();
               OnChanged(new EventArgs() { });
          }
     }
}