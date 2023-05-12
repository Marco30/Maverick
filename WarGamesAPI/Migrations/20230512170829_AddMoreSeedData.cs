using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Conversation",
                columns: new[] { "Id", "Date", "Name", "Updated", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fun chat", new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 2, new DateTime(2023, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Questions", new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 3, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "My first Conversation", new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "ConversationId", "Date", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "How many babies can a rabbit have in its lifetime??", 5 },
                    { 2, 2, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the difference between a struct and a class in C#?", 5 },
                    { 3, 1, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is AI?", 5 }
                });

            migrationBuilder.InsertData(
                table: "Answer",
                columns: new[] { "Id", "Date", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "AI stands for Artificial Intelligence. It refers to the simulation of human intelligence in machines that are programmed to think and learn like humans. AI involves the development of computer systems and algorithms that can perform tasks that typically require human intelligence, such as visual perception, speech recognition, decision-making, problem-solving, and language understanding.\r\n\r\nAI can be classified into two broad categories: Narrow AI and General AI. Narrow AI, also known as weak AI, is designed to perform a specific task or a set of tasks. Examples of narrow AI include virtual personal assistants (like Siri or Alexa), image recognition systems, and recommendation algorithms.\r\n\r\nOn the other hand, General AI, also known as strong AI or AGI (Artificial General Intelligence), refers to AI systems that possess the ability to understand, learn, and apply knowledge across multiple domains, similar to human intelligence. However, achieving General AI is still a significant challenge and an ongoing area of research.\r\n\r\nAI utilizes various techniques and approaches, including machine learning, deep learning, natural language processing, computer vision, and robotics. These techniques enable AI systems to process large amounts of data, extract patterns, and make predictions or decisions based on the analyzed information.\r\n\r\nAI has a wide range of applications across various fields, including healthcare, finance, transportation, manufacturing, customer service, and many more. Its potential impact on society and the economy is substantial, with both positive advancements and potential ethical considerations that need to be addressed as AI continues to evolve." },
                    { 2, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "In C#, both structs and classes are used to define custom data types, but they have some fundamental differences in their behavior and usage. Here are the key differences between structs and classes in C#:\r\n\r\nValue type vs. Reference type: A struct is a value type, while a class is a reference type. When you create a variable of a struct type, the value is stored directly in the variable. In contrast, when you create a variable of a class type, the variable holds a reference (memory address) to the actual object.\r\n\r\nMemory allocation: Structs are usually allocated on the stack, and their memory is managed automatically by the runtime. Classes, on the other hand, are allocated on the heap, and their memory is managed using garbage collection.\r\n\r\nDefault behavior: Structs have value semantics, meaning that when you assign a struct variable to another, a copy of the value is made. Each variable holds its own independent copy. Classes, however, have reference semantics. When you assign a class variable to another, both variables point to the same object in memory. Modifying one variable affects the other because they refer to the same object.\r\n\r\nPerformance: Structs can be more efficient in terms of memory usage and performance for small, simple types because they are allocated on the stack and involve fewer memory management operations. Classes, being reference types, involve indirection and heap allocation, which can introduce overhead.\r\n\r\nInheritance and Polymorphism: Classes support inheritance, allowing you to create hierarchies of classes and utilize polymorphism through virtual and override keywords. Structs do not support inheritance or polymorphism. They are sealed and cannot be used as base classes.\r\n\r\nNullable types: Structs can be made nullable using the \"?\" suffix, allowing them to have a value or be null. Classes are reference types and are nullable by default.\r\n\r\nUsage scenarios: Structs are commonly used for small, lightweight types representing simple data values, such as coordinates, dates, or currency amounts. They are suitable when you need value semantics, want to avoid heap allocation, or have specific performance requirements. Classes are typically used for more complex objects, modeling entities, and providing behavior and functionality through methods and properties.\r\n\r\nIt's important to consider these differences when deciding whether to use a struct or a class, as they have implications for memory management, performance, and the intended behavior of your code." },
                    { 3, new DateTime(2023, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Rabbits are known for their high reproductive potential. The number of babies a rabbit can have in its lifetime depends on various factors, including the specific species of rabbit, its health, environment, and reproductive capabilities.\r\n\r\nGenerally, rabbits are prolific breeders and have short gestation periods. On average, a female rabbit, also known as a doe, can have multiple litters per year. The gestation period for rabbits ranges from around 28 to 35 days, depending on the species.\r\n\r\nA typical litter size for rabbits can range from 1 to 14 or more, depending on the breed and individual factors. However, the average litter size is usually between 4 and 8 kits (baby rabbits).\r\n\r\nAssuming a conservative estimate of 4 litters per year and an average litter size of 6 kits, a rabbit could potentially have around 24 offspring in a single year. Considering that rabbits can breed for several years, with a lifespan of approximately 5 to 10 years or more, the total number of babies a rabbit can have in its lifetime can be quite significant, potentially reaching several hundred offspring.\r\n\r\nIt's worth noting that these numbers are approximate and can vary based on different factors, including the rabbit's health, breeding conditions, and human intervention in managing their reproductive cycles." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Answer",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Conversation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Conversation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Conversation",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
