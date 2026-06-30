export default function Settings() {
  return (
    <div>
      <h2>Settings</h2>
      <div className="settings-list">
        <div className="settings-item">
          <h3>Change Password</h3>
          <p>Update the password used to sign in to your account.</p>
        </div>
        <div className="settings-item">
          <h3>Notification Preferences</h3>
          <p>Choose which course updates and reminders you receive.</p>
        </div>
        <div className="settings-item">
          <h3>Theme Preferences</h3>
          <p>Switch between light and dark appearance for the portal.</p>
        </div>
      </div>
    </div>
  );
}
