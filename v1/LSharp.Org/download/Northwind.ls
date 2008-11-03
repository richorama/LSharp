;;; Northwind Database Example
(reference 
	"System.Data")

(using 
	"System.Data" 
	"System.Data.SqlClient")

;; Given a database connection string and a sql expression,
;; runs the SQL and returns a dataset
(= get-dataset (fn (connection-string sql)
	(do
		(Open (= cn (new SqlConnection connection-string)))
		(= cmd (new SqlDataAdapter sql cn))
		(= ds (new DataSet))
		(fill cmd ds)
		(dispose cmd)
		(dispose cn)
		ds)))
	


;; Of course your SQL Server wont have a blank SA password, so edit this
;; connection-string accordingly ..
(= connection-string "server=localhost;user ID=sa;password=;database=Northwind")

;; Get the list of customers
(= my-dataset (get-dataset connection-string "SELECT * FROM Customers"))
(= my-table (Item (Tables my-dataset) 0))

;; Filter the table further
(= filteredTable (Select my-table "CompanyName LIKE 'A%'"))

;; Print the rows
(each row filteredTable
	(prl (the Cons row)))
