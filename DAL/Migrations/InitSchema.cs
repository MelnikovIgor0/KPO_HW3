using FluentMigrator;

namespace DAL.Migrations;

[Migration(228228227, TransactionBehavior.None)]
public sealed class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt32().PrimaryKey("user_pk").Unique().NotNullable()
            .WithColumn("username").AsString(50).Unique().NotNullable()
            .WithColumn("email").AsString(100).Unique().NotNullable()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("role").AsString(10).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().NotNullable();
        Create.Table("sessions")
            .WithColumn("id").AsInt32().PrimaryKey("session_pk").Unique().NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable()
            .WithColumn("session_token").AsString(4096).NotNullable()
            .WithColumn("expires_at").AsDateTime().NotNullable();
        Create.Table("dishes")
            .WithColumn("id").AsInt32().PrimaryKey("dish_pk").Unique().NotNullable()
            .WithColumn("name").AsString(50).Unique().NotNullable()
            .WithColumn("description").AsString(1000).NotNullable()
            .WithColumn("price").AsDecimal().NotNullable()
            .WithColumn("quantity").AsInt32().NotNullable();
        Create.Table("orders")
            .WithColumn("id").AsInt32().PrimaryKey("order_pk").Unique().NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable()
            .WithColumn("status").AsString(30).NotNullable()
            .WithColumn("special_requests").AsString(1000).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().NotNullable();
        Create.Table("order_dishes")
            .WithColumn("id").AsInt32().PrimaryKey("order_dish_pk").Unique().NotNullable()
            .WithColumn("order_id").AsInt32().NotNullable()
            .WithColumn("dish_id").AsInt32().NotNullable()
            .WithColumn("quantity").AsInt32().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("user");
        Delete.Table("session");
        Delete.Table("dishes");
        Delete.Table("orders");
        Delete.Table("order_dishes");
    }
}
