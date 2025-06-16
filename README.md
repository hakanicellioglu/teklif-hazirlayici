# Teklif Hazırlayıcı

**Teklif Hazırlayıcı**, işletmelerin müşterilerine hızlı ve profesyonel teklifler sunmalarını kolaylaştıran bir masaüstü uygulamasıdır. Bu araç, teklif oluşturma sürecini otomatikleştirerek zaman tasarrufu sağlar ve hata riskini azaltır.

## Özellikler

- **Kullanıcı Dostu Arayüz**: Basit ve sezgisel tasarımı sayesinde kullanıcılar tekliflerini kolayca oluşturabilir.
- **Veri Yönetimi**: Müşteri ve ürün bilgilerini veritabanında saklayarak tekrar kullanım imkanı sunar.
- **PDF Oluşturma**: Hazırlanan teklifleri PDF formatında dışa aktararak paylaşımı kolaylaştırır.
- **Şablon Desteği**: Farklı teklif şablonları ile özelleştirilmiş teklifler oluşturabilirsiniz.
- **Otomatik Güncelleme**: Uygulama açılışta en son sürümü kontrol eder ve gerekirse güncellemeyi indirip yükler.

## Kurulum

1. **Depoyu Klonlayın**:
   ```bash
   git clone https://github.com/hakanicellioglu/teklif-hazirlayici.git
   ```

2. **Proje Dosyasını Açın**:
   - Visual Studio veya uyumlu bir IDE kullanarak `Teklif Hazırlayıcı.sln` dosyasını açın.

3. **Bağımlılıkları Yükleyin**:
   - Gerekli NuGet paketlerini yükleyin. (Örneğin, `packages.config` dosyasını kullanarak.)

4. **Bağlantı Dizesini Tanımlayın**:
   - `SQL_CONN_STRING` ortam değişkenini kendi veritabanı bilgilerinizle ayarlayın **veya**
     `App.config` ile aynı klasörde `App.config.user` dosyası oluşturarak `SqlConnectionString` değerini burada belirtin.

5. **Projeyi Derleyin ve Çalıştırın**:
   - Projeyi derleyin ve uygulamayı başlatın.

## Kullanım

1. Uygulamayı başlatın.
2. Yeni bir teklif oluşturmak için "Yeni Teklif" butonuna tıklayın.
3. Müşteri ve ürün bilgilerini girin.
4. Teklifi kaydedin ve PDF olarak dışa aktarın.

## Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz! Lütfen aşağıdaki adımları izleyerek katkıda bulunabilirsiniz:

1. Bu depoyu forklayın.
2. Yeni bir dal oluşturun:
   ```bash
   git checkout -b ozellik/yenilik
   ```
3. Değişikliklerinizi yapın ve commit edin:
   ```bash
   git commit -m "Yeni özellik eklendi"
   ```
4. Dalınızı push edin:
   ```bash
   git push origin ozellik/yenilik
   ```
5. Bir pull request oluşturun.

## Lisans

Bu proje Apache-2.0 Lisansı ile lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakınız.
