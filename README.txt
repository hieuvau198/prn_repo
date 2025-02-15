1. Tạo chứng chỉ SSL bằng PowerShell
Mở PowerShell dưới quyền Administrator và chạy lệnh sau để tạo chứng chỉ:

New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "Cert:\LocalMachine\My"

Giải thích:
New-SelfSignedCertificate : Lệnh tạo chứng chỉ tự ký.
-DnsName "localhost" : Chứng chỉ chỉ áp dụng cho tên miền localhost.
-CertStoreLocation "Cert:\LocalMachine\My" : Lưu chứng chỉ vào thư mục Local Machine.
Kết quả sẽ hiện ra một chứng chỉ mới trong Certificate Store.

2. Xuất chứng chỉ thành file PFX
Sau khi tạo chứng chỉ, bạn cần xuất nó ra file .pfx để sử dụng trong C#.

Bước 1: Mở Certificate Manager
Nhấn Win + R → gõ certmgr.msc → nhấn Enter.
Điều hướng đến thư mục Certificates - Local Computer → Personal → Certificates.
Tìm chứng chỉ mới tạo (tên "localhost").

Bước 2: Xuất chứng chỉ
Nhấp chuột phải vào chứng chỉ localhost → All Tasks → Export.
Chọn Yes, export the private key.
Chọn định dạng .PFX.
Đặt mật khẩu (ví dụ: "password").
Lưu file thành "server.pfx".

3. Cài đặt chứng chỉ vào Trusted Root
Nếu không cài đặt vào Trusted Root, client có thể không tin tưởng chứng chỉ.
Thực hiện các bước sau:

Mở Certificate Manager (certmgr.msc).
Vào Personal → Certificates, chọn chứng chỉ localhost.
Nhấp chuột phải → Copy.
Chuyển đến thư mục Trusted Root Certification Authorities → Certificates.
Nhấp chuột phải vào danh sách → Paste.
Xác nhận cài đặt.

4. Sử dụng chứng chỉ trong C#
Sau khi có file server.pfx, cập nhật code server để tải chứng chỉ:


X509Certificate2 serverCertificate = new X509Certificate2("server.pfx", "password");


Nếu gặp lỗi quyền truy cập, hãy thử chạy chương trình với quyền Administrator.
Nếu client báo lỗi không tin tưởng chứng chỉ, kiểm tra xem chứng chỉ đã được cài đặt vào Trusted Root chưa.
