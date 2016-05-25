using BloodAlcoholCalculator.Model;
using System;
using System.Data;
using System.Data.Linq;
using System.Linq;

namespace BloodAlcoholCalculator.Data
{
     public class MainDatabaseContext : DataContext
     {
          public Table<User> Users;
          public Table<ConsumedDrink> ConsumedDrinks;
          public Table<DefinedDrink> DefinedDrinks;

          public MainDatabaseContext(IDbConnection connection) : base(connection)
          {
          }
     }

     public class MainDatabase : Database
     {
          private MainDatabaseContext databaseContext;

          public MainDatabase(string filename)
          {
               string connectionString = String.Format("Data Source ={0};Version=3;", filename);
               base.Connect(connectionString);
               InitializeContext();

               CreateTables();
          }

          public override void CreateTables()
          {
               base.CreateTable(typeof(User).GetProperties(), "User");
               base.CreateTable(typeof(ConsumedDrink).GetProperties(), "ConsumedDrink");
               base.CreateTable(typeof(DefinedDrink).GetProperties(), "DefinedDrink");
          }

          public override void InitializeContext()
          {
               databaseContext = new MainDatabaseContext(connection);
               context = databaseContext;
          }

          public void Add<Type>(Type t)
          {
               if (t.GetType() == typeof(ConsumedDrink)) {
                    Add(t as ConsumedDrink);
               } else if (t.GetType() == typeof(DefinedDrink)) {
                    Add(t as DefinedDrink);
               } else if (t.GetType() == typeof(User)) {
                    Add(t as User);
               } else {
                    throw new InvalidOperationException("Unsupported type");
               }
          }

          public MainDatabaseContext Context => databaseContext;

          public void Remove(object elem)
          {
               var table = databaseContext.GetTable(elem.GetType());
               table?.DeleteOnSubmit(elem);
               databaseContext.SubmitChanges();
          }

          //attach entitiy so edits can be made
          //unsure if needed
          public void Attach(object elem)
          {
               var table = databaseContext.GetTable(elem.GetType());
               table.Attach(elem, asModified: true);
               databaseContext.Refresh(RefreshMode.KeepCurrentValues, elem);
               databaseContext.SubmitChanges();
          }

          //
          //ConsumedDrink
          //
          public void Add(ConsumedDrink drink)
          {
               //get id
               long largestId = 0;
               var idQuery = from x in databaseContext.ConsumedDrinks
                             orderby x.Id descending
                             select x;
               if (idQuery.Any()) {
                    largestId = idQuery.Max(x => x.Id) + 1;
               }
               drink.Id = largestId;
               databaseContext.ConsumedDrinks.InsertOnSubmit(drink);
               databaseContext.SubmitChanges();
          }

          //
          //DefinedDrink
          //
          public void Add(DefinedDrink drink)
          {
               //get id
               long largestId = 0;
               var idQuery = from x in databaseContext.DefinedDrinks
                             orderby x.Id descending
                             select x;
               if (idQuery.Any()) {
                    largestId = idQuery.Max(x => x.Id) + 1;
               }
               drink.Id = largestId;
               databaseContext.DefinedDrinks.InsertOnSubmit(drink);
               databaseContext.SubmitChanges();
          }

          //
          //User
          //
          public void Add(User user)
          {
               //get id
               long largestId = 0;
               var idQuery = from x in databaseContext.Users
                             orderby x.Id descending
                             select x;
               if (idQuery.Any()) {
                    largestId = idQuery.Max(x => x.Id) + 1;
               }
               user.Id = largestId;
               databaseContext.Users.InsertOnSubmit(user);
               databaseContext.SubmitChanges();
          }

          public void SubmitChanges()
          {
               databaseContext.SubmitChanges();
          }
     }
}